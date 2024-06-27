using ECommerceAPI.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Services
{
    public interface IAuthService
    {
        Task<DTO.Token> LoginAsync(string usernameOrEmail,string password, int accessTokenLifetime);
    }
}
