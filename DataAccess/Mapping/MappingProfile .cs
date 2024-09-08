using AutoMapper;
using System.Net.Mail;
using System.Numerics;
using System.Xml.Linq;
using System;
using System.Net.Sockets;
using DataAccess.DTOs;
using DataAccess.Models;

namespace DataAccess.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AppUser, GetUserDto>().ReverseMap();
            CreateMap<AppUser, PutUserDto>().ReverseMap();
            CreateMap<AppUser, PostUserDto>().ReverseMap();
            CreateMap<AppUser, GetOneUserDto>().ReverseMap();


            CreateMap<Product, PostProdcutDto>().ReverseMap();

        
            CreateMap<Brand, PostBrandDto>().ReverseMap();


            CreateMap<Style,PostStyleDto>().ReverseMap();


            CreateMap<Material,PostMaterialDto>().ReverseMap();


            CreateMap<Category,PostCategoryDto>().ReverseMap();


            CreateMap<Product, GetProductDto>().ReverseMap();
            CreateMap<ProductImage, ImageDto>().ReverseMap();

            CreateMap<ServiceRequest, PostServiceDto>().ReverseMap();
            CreateMap<ServiceImage, ImageDto>().ReverseMap();
            CreateMap<ServiceRequest, GetServiceDto>().ReverseMap();

        }
    }
}
