using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

/// <summary>
/// Klasa namenjena za pristupanje bazi preko Entity Framework-a
/// </summary>
namespace ShoppingCart.Models.Data
{
    public class ShoppingCartDB : DbContext
    {
        //Setujemo pages za tblPages
        public DbSet<PageDTO> Pages { get; set; }
        //set za tblSidebar
        public DbSet<SidebarDTO> Sidebar { get; set; }
        //set za tblCategories
        public DbSet<CategoriesDTO> Categories { get; set; }
        //set za tblProducts
        public DbSet<ProductsDTO> Products { get; set; }
        //set za Users tabelu
        public DbSet<UserDTO> Users { get; set; }
        //set za Roles tabelu
        public DbSet<RolesDTO> Roles { get; set; }
        //set za User Roles tableu
        public DbSet<UserRolesDTO> UserRoles { get; set; }
        //set za Orders tableu
        public DbSet<OrdersDTO> Orders { get; set; }
        //set za OrderDetail tabelu
        public DbSet<OrderDetailsDTO> OrderDetails { get; set; }


    }
}