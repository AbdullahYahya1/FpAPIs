using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using NLog.Web;
using System.Text;
using Business;
using DataAccess.IRepositories;
using DataAccess.Models;
using DataAccess.Repositories;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Business.EmailSender;
using Hangfire;
using Business.BackgroundJobService;
using Hangfire.MemoryStorage;
using Business.IServices;
using Business.Services;
using DataAccess.IRepositories;
using DataAccess.Mapping;
using DataAccess.Context;
using Business.Hubs;
using Microsoft.AspNetCore.Mvc.Filters;

var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
try
{
    logger.Debug("Application Starting Up");
    // Ensure logs directory exists 
    var logDir = Path.Combine(Directory.GetCurrentDirectory(), "logs");
    if (!Directory.Exists(logDir))
    {
        Directory.CreateDirectory(logDir);
    }
    var builder = WebApplication.CreateBuilder(args);
    //hangfire

    builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseMemoryStorage());
    builder.Services.AddHangfireServer();
    builder.Services.AddSingleton<BackgroundJobService>();

    builder.Services.AddDbContext<FPDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
    builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSignalR() ;

    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "APIs", Version = "v1" });
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = @"JWT Authorization header using the Bearer scheme.  
                          Enter 'Bearer' [space] example : 'Bearer 12345abcdef'",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement()
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    },
                    Scheme = "oauth2",
                    Name = "Bearer",
                    In = ParameterLocation.Header,
                },
                new List<string>()
            }
        });
    });
    builder.Services.AddTransient<IEmailSender, EmailSender>();
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowSpecificOrigin",
            policyBuilder => policyBuilder.WithOrigins("http://localhost:4200", "https://resonant-melomakarona-f57d6c.netlify.app")
                                          .AllowAnyHeader()     
                                          .AllowAnyMethod()
                                          .AllowCredentials());
    });


    builder.Services.AddControllers().AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
    });


    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];
                if (!string.IsNullOrEmpty(accessToken) &&
                    context.HttpContext.Request.Path.StartsWithSegments("/userHub"))
                {
                    context.Token = accessToken;
                }
                return Task.CompletedTask;
            }
        };
    });

    // Register  
    builder.Services.AddScoped<IUserService, UserService>();
    builder.Services.AddScoped<IProductService, ProductService>();
    builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
    builder.Services.AddScoped<IUserRepository, UserRepository>();
    builder.Services.AddScoped<IProductRepository, ProductRepository>();
    builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
    builder.Services.AddScoped<IStyleRepository, StyleRepository>();
    builder.Services.AddScoped<IMaterialRepository, MaterialRepository>();
    builder.Services.AddScoped<IBrandRepository, BrandRepository>();
    builder.Services.AddScoped<IServiceRequestService, ServiceRequestService>();
    builder.Services.AddScoped<IServiceRequestRepository, ServiceRequestRepository>();

    builder.Services.AddScoped<IWishlistItemRepository, WishlistItemRepository>();
    builder.Services.AddScoped<IWishlistItemService, WishlistItemService>();
    builder.Services.AddScoped<ICartItemService, CartItemService>();
    builder.Services.AddScoped<ICartItemRepository, CartItemRepository> ();

    builder.Services.AddScoped<IOrderService, OrderService>();
    builder.Services.AddScoped<IOrderRepository, OrderRepository>();
    builder.Services.AddScoped<IUserAddressRepository, UserAddressRepository>();
    builder.Services.AddScoped<IUserAddressService, UserAddressService>();

    builder.Services.AddScoped<IUserPurchaseTransactionRepository, UserPurchaseTransactionRepository>();
    builder.Services.AddScoped<IUserPurchaseTransactionService, UserPurchaseTransactionService>();
    builder.Services.AddScoped<INotificationService, NotificationService>();
    
    #region HealthChecks

    builder.Services.AddHealthChecks()
        .AddSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
        .AddDbContextCheck<FPDbContext>(); ;
    #endregion


    // Configure logging 
    builder.Logging.ClearProviders();
    builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
    builder.Logging.AddConsole();
    builder.Host.UseNLog();

    var app = builder.Build();

    // Configure the HTTP request pipeline. 
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    app.UseHttpsRedirection();
 
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapHub<BroadcastHub>("/broadcastHub");
    app.MapHub<AdminHub>("/Admin").AllowAnonymous();


    //app.MapHealthChecks("/HealthCheck/Check");


    app.MapControllers();
    app.UseStaticFiles();
    app.UseCors("AllowSpecificOrigin");
    //hangfire
    app.Lifetime.ApplicationStarted.Register(() =>
    {
        using (var scope = app.Services.CreateScope())
        {
            var recurringJobManager = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();
            var backgroundJobService = scope.ServiceProvider.GetRequiredService<BackgroundJobService>();
            recurringJobManager.AddOrUpdate("healthCheck-job", () => backgroundJobService.HealthCheck(), Cron.Minutely);
            recurringJobManager.AddOrUpdate("cancel-orders-job",() => backgroundJobService.CancelOrders(),Cron.Minutely);

        }
    });

    app.Run();
}
catch (Exception exception)
{
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    NLog.LogManager.Shutdown();
}