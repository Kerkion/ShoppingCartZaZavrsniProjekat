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

    }
}