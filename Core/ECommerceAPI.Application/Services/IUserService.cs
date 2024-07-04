using ECommerAPI.Domain.Entities.Identity;
using ECommerceAPI.Application.DTO.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Services
{
    public interface IUserService
    {
        Task<CreateUserResponse> CreateAsync(CreateUser model);
        Task UpdateRefreshToken(string refreshToken, User user, DateTime accesTokenDate, int refreshTokenLifeTime);
    }
}
