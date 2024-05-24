using ECommerAPI.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerAPI.Domain.Entities
{
    public class SiteFile : BaseEntity
    {
        public string Name {  get; set; }
        public string Path { get; set; }
        [NotMapped]
        public override DateTime LastModifiedDate { get => base.LastModifiedDate; set => base.LastModifiedDate = value; }
    }
}
