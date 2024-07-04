using ECommerceAPI.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Cqrs.Commands.User.RefreshToken
{
    public class RefreshTokenCommandResponse
    {
        public DTO.Token Token { get; set; }
    }
}
