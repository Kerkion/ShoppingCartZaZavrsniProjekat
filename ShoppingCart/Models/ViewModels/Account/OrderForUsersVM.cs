using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingCart.Models.ViewModels.Account
{
    public class OrderForUsersVM
    {
        public int OrderNumber { get; set; }
        public decimal Total { get; set; }
        public Dictionary<string, int> ProductsAndQuantity { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}