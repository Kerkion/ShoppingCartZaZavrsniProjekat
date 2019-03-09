using ShoppingCart.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

/// <summary>
/// Ovo ce biti ViewModel za Admin Page koji je povezan class-om PageDTO
/// </summary>
namespace ShoppingCart.Models.ViewModels.Pages
{
    public class PageVM
    {
        //Osnovni Konstruktor
        public PageVM()
        {

        }
        //Pravimo konstruktor koji ce povuci iz PageDTO(Page (DTO je ustvari Data Transfer Object) i popuniti Property-e koji su isti ovde kao i tamo.
        //Prilikom pravljenja ovog objekta bice pozvan PageDTO i on ce napuniti ovaj konstruktor 
        public PageVM(PageDTO row)
        {
            Id = row.Id;
            Title = row.Title;
            Slug = row.Slug;
            Body = row.Body;
            Sorting = row.Sorting;
            HasSidebar = row.HasSidebar;
        }


        public int Id { get; set; }
        //Title ce biti obavezan za stranicu i mora da bude minimalno 3 karaktera duzine i maksimalno 50 karaktera(to znace ove anotacije)
        [Required]
        [StringLength(50,MinimumLength = 3)]
        public string Title { get; set; }
        public string Slug { get; set; }
        //Body ce biti obavezan za stranicu i mora da bude minimalno 3 karaktera duzine i maksimalno Koliko je Maksimalna vrednost inta to jest 2147483647 karaktera (to znace ove anotacije)
        [Required]
        [StringLength(int.MaxValue, MinimumLength = 3)]
        [AllowHtml]
        public string Body { get; set; }
        public int Sorting { get; set; }
        public bool HasSidebar { get; set; }
    }
}