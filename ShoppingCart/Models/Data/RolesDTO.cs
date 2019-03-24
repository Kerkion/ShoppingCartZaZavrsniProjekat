using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShoppingCart.Models.Data
{
    public class RolesDTO
    {
        [Key]
        public int Id { get; set; }
        public string Roles { get; set; }
    }
}