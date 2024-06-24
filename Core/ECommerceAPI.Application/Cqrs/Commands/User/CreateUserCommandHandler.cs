using ECommerceAPI.Application.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Cqrs.Commands.User
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommandRequest, CreateUserCommandResponse>
    {
        readonly UserManager<ECommerAPI.Domain.Entities.Identity.User> _userManager;

        public CreateUserCommandHandler(UserManager<ECommerAPI.Domain.Entities.Identity.User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<CreateUserCommandResponse> Handle(CreateUserCommandRequest request, CancellationToken cancellationToken)
        {
            IdentityResult result = await _userManager.CreateAsync(new()
            {
                Id =Guid.NewGuid().ToString(),
                Name = request.Name,
                Email = request.Email,
                UserName = request.Username,
                
                
            }, request.Password);

            CreateUserCommandResponse response = new() { Succeeded = result.Succeeded };
            if(result.Succeeded)
            {
                response.Message = "Kullanıcı başarıyla kayıt oldu.";
            }
            else{
                foreach(var error in result.Errors)
                {
                    response.Message += $"{error.Code}-{error.Description}";
                }
            }
            return response;

        }
    }
}
