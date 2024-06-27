using ECommerceAPI.Application.DTO.User;
using ECommerceAPI.Application.Exceptions;
using ECommerceAPI.Application.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Cqrs.Commands.User.UserCreate
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommandRequest, CreateUserCommandResponse>
    {
        readonly IUserService _userService;

        public CreateUserCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<CreateUserCommandResponse> Handle(CreateUserCommandRequest request, CancellationToken cancellationToken)
        {
            CreateUserResponse response = await _userService.CreateAsync(new()
            {
                Name = request.Name,
                Email = request.Email,
                Username = request.Username,
                Password = request.Password,
                PasswordConfirm = request.PasswordConfirm,
            });
            return new()
            {
                Message = response.Message,
                Succeeded = response.Succeeded,
            };

        }
    }
}
