using ShoppingCart.Models.Data;
using ShoppingCart.Models.ViewModels.Pages;
using ShoppingCart.Models.ViewModels.Shop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace ShoppingCart.Areas.Admin.Controllers
{
    public class ShopController : Controller
    {
        // GET: Admin/Shop/Categories
        public ActionResult Categories()
        {
            //Deklaracija liste category VM-a
            List<CategoriesVM> categoryVMList;

            using (ShoppingCartDB db = new ShoppingCartDB())
            {
                //Pronadji sve kategorije na osnovu zadatog vm-a i sortiraj ih prema sortingu iz tabele
                categoryVMList = db.Categories.ToArray().OrderBy(x => x.Sorting).Select(x => new CategoriesVM(x)).ToList();
            };
            //vrati listu view-a
            return View(categoryVMList);
        }
    }
}