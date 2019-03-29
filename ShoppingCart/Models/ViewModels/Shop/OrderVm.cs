using ShoppingCart.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingCart.Models.ViewModels.Shop
{
    public class OrderVm
    {
        public OrderVm()
        {

        }

        public OrderVm(OrdersDTO row)
        {
            OrderID = row.OrderID;
            UserID = (int)row.UserID;
            CreatedAt = (DateTime)row.CreatedAt;
        }

        public int OrderID { get; set; }
        public int UserID { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}