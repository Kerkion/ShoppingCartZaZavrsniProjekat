using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


/// <summary>
/// Ovo je DTO class-a koja ce komunicirati sa tblPages u bazi podataka
/// </summary>
namespace ShoppingCart.Models.Data
{
    //Page DTO(DTO =  DATA TRANSFER OBJECT) nam sluzi da prebacimo sve iz baze podataka u property koje smo napravili.
    //Data anotacije [Table("tblPages")] je koriscena zato sto se ime ove klase ne poklapa sa imenom table u bazi podataka pa entity framework ne bih znao na koju tableu pokusavamo da povezemo ovu klasu
    [Table("tblPages")]
    public class PageDTO
    {
        //Data Anotacija za primarni kljuc
        //predstavlja ce Id nase stranice
        [Key]
        public int Id { get; set; }
        //Predstaclja ce Title nase stranice
        public string Title { get; set; }
        //Predstavlja ce Slug nase stranice(Slug je  deo url-a koji indentifikuje stranicu,tako sto ce biti napisano da covek moze da razume
        public string Slug { get; set; }
        //Predstavljace Body nase stranice
        public string Body { get; set; }
        //Predstavlja ce sortiranje nasih stranica(jednostavnije receno ako budemo imali vise stranica mocicemo da ih sortiramo kako zelimo a sa ovim propertiem cemo pratiti gde se nalazi koji)
        public int Sorting { get; set; }
        //Predstavlja Side bar na nasoj stranici(mocicemo da biramo da li ce nasa stranica imati side bar ili ne)
        public bool HasSidebar { get; set; }
    }
}