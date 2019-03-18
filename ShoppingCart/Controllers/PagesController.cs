using ShoppingCart.Models.Data;
using ShoppingCart.Models.ViewModels.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShoppingCart.Controllers
{
    public class PagesController : Controller
    {
        // GET: Index/{page}
        public ActionResult Index(string page = "")
        {
            //uzeti ili setovati page slug
            if (page == "")
            {
                page = "home";
            }
            //deklaristati model i DTO
            PageVM model;
            PageDTO dto;
            using (ShoppingCartDB db = new ShoppingCartDB())
            {
                //proveriti da li page postoji
                if (!db.Pages.Any(x => x.Slug.Equals(page)))
                {
                    return RedirectToAction("Index", new { page = "" });
                }
            }
            using (ShoppingCartDB db = new ShoppingCartDB())
            {
                //Uzeti PageDTO
                dto = db.Pages.Where(x => x.Slug == page).FirstOrDefault();
            }
            //Postaviti title stranice
            ViewBag.PageTitle = dto.Title;
            //Proveriti da li ima sidebar
            if (dto.HasSidebar)
            {
                ViewBag.Sidebar = "Yes";
            }
            else
            {
                ViewBag.Sidebar = "No";
            }
            //Inicijalizovati model
            model = new PageVM(dto);
            //vratiti model sa view-om
            return View(model);
        }


        public ActionResult PagesMenuPartialView()
        {
            //Deklarisati listu pageVM
            List<PageVM> pageList;
            //Pronaci sve pageve osim home
            using (ShoppingCartDB db = new ShoppingCartDB())
            {
                pageList = db.Pages.ToArray().OrderBy(x => x.Sorting).Where(x => x.Slug != "home").Select(x => new PageVM(x)).ToList();
            }
            //vratiti partial sa listom
            return PartialView(pageList);
        }
    }
}