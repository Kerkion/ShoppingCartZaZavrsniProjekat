using ShoppingCart.Models.Data;
using ShoppingCart.Models.ViewModels.Shop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShoppingCart.Controllers
{
    public class ShopController : Controller
    {
        // GET: Shop
        public ActionResult Index()
        {
            return RedirectToAction("Index","Pages");
        }

        //partial view za Category
        public ActionResult CategoryMenuPartialView()
        {
            //Deklarisanje  modela
            List<CategoriesVM> categoryVmList;
            //Inicijalizacija modela
            using (ShoppingCartDB db = new ShoppingCartDB())
            {
                categoryVmList = db.Categories.ToArray().OrderBy(x => x.Sorting).Select(x => new CategoriesVM(x)).ToList();
            }
            //Vrati partial view sa modelom

          
            return PartialView(categoryVmList);
        }
    }
}