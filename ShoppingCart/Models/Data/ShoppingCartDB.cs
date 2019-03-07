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
        //Setujemo pages
        public DbSet<PageDTO> Pages { get; set; }
    }
}