﻿using ShoppingCart.Models.Data;
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

        //GET : shop/category/name
        public ActionResult Categories(string name)
        {
            //Deklarisati listu ProductVM
            List<ProductsVM> productsList;
            using (ShoppingCartDB db = new ShoppingCartDB())
            {
                //Pronaci category id
                CategoriesDTO dto = db.Categories.Where(x => x.Slug == name).FirstOrDefault();
                int catId = dto.Id;
                //Inicijalizovati listu
                productsList = db.Products.ToArray().Where(x => x.CategoryId == catId).Select(x => new ProductsVM(x)).ToList();
                //pronaci category name
                var productCategory = db.Products.Where(x => x.CategoryId == catId).FirstOrDefault();
                //Koristicemo za title
                ViewBag.CatName = productCategory.CategoryName;
            }

                //vratit View sa listom
                return View(productsList);
        }
    }
}