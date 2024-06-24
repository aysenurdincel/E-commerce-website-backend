using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerAPI.Domain.Entities.Identity
{
    public class User : IdentityUser<string>
    {
        public string Name {  get; set; }
        
    }
}
