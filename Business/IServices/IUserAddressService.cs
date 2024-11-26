using DataAccess.DTOs;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.IServices
{
    public interface IUserAddressService:IService<UserAddress>
    {
        Task<ResponseModel<bool>> AddAddress(PostAddressDto postAddressDto);
        Task<ResponseModel<List<GetAddressDto>>> GetAddresses();

        Task<ResponseModel<bool>> UpdateAddress(UpdateAddressDto updateAddressDto);
    }
}
