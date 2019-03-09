using ShoppingCart.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingCart.Models.ViewModels.Pages
{
    public class SidebarVM
    {
        //Standardan konstrukor
        public SidebarVM()
        {

        }
        //Konstruktor koji prima DTO model
        public SidebarVM(SidebarDTO row)
        {
            Id = row.Id;
            Body = row.Body;
        }

        public int Id { get; set; }
        public string Body { get; set; }
    }
}