using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ShoppingCart.Models.Data
{
    [Table("tblUserRoles")]
    public class UserRolesDTO
    {
        [Key, Column(Order = 0)]
        public int UserId { get; set; }
        [Key, Column(Order = 1)]
        public int RoleID { get; set; }

        [ForeignKey("UserId")]
        public virtual UserDTO User { get; set; }
        [ForeignKey("RoleID")]
        public virtual RolesDTO Role { get; set; }
    }
}