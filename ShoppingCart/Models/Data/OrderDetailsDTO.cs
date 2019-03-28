using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ShoppingCart.Models.Data
{
    [Table("tblOrderDetails")]
    public class OrderDetailsDTO
    {
        [Key]
        public int ID { get; set; }
        public int? OrderID { get; set; }
        public int? UserID { get; set; }
        public int? ProductID { get; set; }
        public int Quantity { get; set; }

        [ForeignKey("OrderID")]
        public virtual OrdersDTO Orders { get; set; }
        [ForeignKey("UserID")]
        public virtual UserDTO Users { get; set; }
        [ForeignKey("ProductID")]
        public virtual ProductsDTO Products { get; set; }
    }
}