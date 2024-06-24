using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Exceptions
{
    public class UserCreationFail : Exception
    {
        public UserCreationFail() : base("Kullanıcı oluşturulamadı.")
        {
        }

        public UserCreationFail(string? message) : base(message)
        {
        }

        public UserCreationFail(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
