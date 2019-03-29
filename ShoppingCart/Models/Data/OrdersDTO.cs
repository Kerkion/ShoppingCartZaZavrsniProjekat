using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ShoppingCart.Models.Data
{
    [Table("tblOrders")]
    public class OrdersDTO
    {
        [Key]
        public int OrderID { get; set; }
        public int? UserID { get; set; }
        public DateTime? CreatedAt { get; set; }

        [ForeignKey("UserID")]
        public virtual UserDTO Users { get; set; }

    }
}