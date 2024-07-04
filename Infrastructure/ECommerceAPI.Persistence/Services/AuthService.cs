using ECommerAPI.Domain.Entities.Identity;
using ECommerceAPI.Application.Cqrs.Commands.User.UserLogin;
using ECommerceAPI.Application.DTO;
using ECommerceAPI.Application.Services;
using ECommerceAPI.Application.Token;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Persistence.Services
{
    public class AuthService : IAuthService
    {
        readonly UserManager<ECommerAPI.Domain.Entities.Identity.User> _userManager;
        readonly SignInManager<ECommerAPI.Domain.Entities.Identity.User> _signInManager;
        readonly ITokenHandler _tokenHandler;
        readonly IUserService _userService;

        public AuthService(UserManager<User> userManager, SignInManager<User> signInManager, ITokenHandler tokenHandler, IUserService userService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenHandler = tokenHandler;
            _userService = userService;
        }

        public async Task<Token> LoginAsync(string usernameOrEmail, string password, int accessTokenLifetime)
        {
            User user = await _userManager.FindByNameAsync(usernameOrEmail);
            if (user == null)            
                user = await _userManager.FindByEmailAsync(usernameOrEmail);
            
            if (user == null)           
                throw new Exception("Kullanıcı bulunamadı.");
            

            SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
            if (result.Succeeded)
            {
                Token token = _tokenHandler.CreateAccessToken(accessTokenLifetime);
                await _userService.UpdateRefreshToken(token.RefreshToken, user, token.Expiration, 5);

                return token;
            }
            throw new Exception();
        }

        public async Task<Token> RefreshTokenLoginAsync(string refreshToken)
        {
            User? user = await _userManager.Users.FirstOrDefaultAsync(users => users.RefreshToken == refreshToken);
            if(user != null && user.RefreshTokenExpirationDate > DateTime.UtcNow)
            {
               Token token =  _tokenHandler.CreateAccessToken(5);
               await _userService.UpdateRefreshToken(token.RefreshToken,user, token.Expiration, 5);

               return token;
            }
            else
                throw new Exception() ;
        }
    }
}
