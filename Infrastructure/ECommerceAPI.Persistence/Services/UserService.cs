using ECommerAPI.Domain.Entities.Identity;
using ECommerceAPI.Application.Cqrs.Commands.User.UserCreate;
using ECommerceAPI.Application.DTO.User;
using ECommerceAPI.Application.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Persistence.Services
{
    public class UserService : IUserService
    {
        readonly UserManager<ECommerAPI.Domain.Entities.Identity.User> _userManager;

        public UserService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<CreateUserResponse> CreateAsync(CreateUser model)
        {
            
            IdentityResult result = await _userManager.CreateAsync(new()
            {
                Id = Guid.NewGuid().ToString(),
                Name = model.Name,
                Email = model.Email,
                UserName = model.Username,


            }, model.Password);

            CreateUserResponse response = new() { Succeeded = result.Succeeded };
            if (result.Succeeded)
            {
                response.Message = "Kullanıcı başarıyla kayıt oldu.";
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    response.Message += $"{error.Code}-{error.Description}";
                }
            }return response;
        }
    }
}
