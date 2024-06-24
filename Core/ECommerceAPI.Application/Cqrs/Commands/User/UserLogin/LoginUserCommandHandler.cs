using ECommerceAPI.Application.Token;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Cqrs.Commands.User.UserLogin
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommandRequest, LoginUserCommandResponse>
    {
        readonly UserManager<ECommerAPI.Domain.Entities.Identity.User> _userManager;
        readonly SignInManager<ECommerAPI.Domain.Entities.Identity.User> _signInManager;
        readonly ITokenHandler _tokenHandler;

        public LoginUserCommandHandler(UserManager<ECommerAPI.Domain.Entities.Identity.User> userManager, SignInManager<ECommerAPI.Domain.Entities.Identity.User> signInManager, ITokenHandler tokenHandler)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenHandler = tokenHandler;
        }

        public async Task<LoginUserCommandResponse> Handle(LoginUserCommandRequest request, CancellationToken cancellationToken)
        {
            ECommerAPI.Domain.Entities.Identity.User user = await _userManager.FindByNameAsync(request.UsernameOrEmail);
            if(user == null)
            {
                user = await _userManager.FindByEmailAsync(request.UsernameOrEmail);
            }
            if(user == null)
            {
                throw new Exception("Kullanıcı bulunamadı.");
            }

            SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, request.Password,false);
            if (result.Succeeded)
            {
                DTO.Token token = _tokenHandler.CreateAccessToken(5);

                return new LoginUserSuccessCommandResponse()
                {
                    Token = token,
                };
            }
            return new LoginUserErrorCommandResponse()
            {
                Message = "Kullanıcı adı veya şifre hatalı"
            };
        }
    }
}
