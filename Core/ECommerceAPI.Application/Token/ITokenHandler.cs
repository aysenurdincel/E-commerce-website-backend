using ECommerAPI.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Token
{
    public interface ITokenHandler
    {
        DTO.Token CreateAccessToken(int expirationMinute, User user);
        string CreateRefreshToken();
    }
}
