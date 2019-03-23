using ShoppingCart.Models.Data;
using ShoppingCart.Models.ViewModels.Shop;
using System;
using System.Collections.Generic;
using System.IO;
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

        //GET : shop/product-details/name
        [ActionName("product-details")]
        public ActionResult ProductDetails(string name)
        {
            //Deklarisati VM i DTO
            ProductsVM model;
            ProductsDTO dto;

            //Inicijalizovati ProductsId
            int id = 0;
            using (ShoppingCartDB db = new ShoppingCartDB())
            {
                //Proveriti da li postoji product
                if(!db.Products.Any(x => x.Slug.Equals(name))){
                    RedirectToAction("Index", "Shop");
                }
                //inicijalizovati DTO
                dto = db.Products.Where(x => x.Slug == name).FirstOrDefault();
                //Uzeti  id
                id = dto.Id;
                //inicijalizovati model
                model = new ProductsVM(dto);
            }

            //Pronaci slike
            //uzeti sve slike iz glaerije
            model.GalleryImages = Directory.EnumerateFiles(Server.MapPath("~/Images/Uploads/Products/" + id + "/Gallery/Thumbs")).Select(x => Path.GetFileName(x));

            //vratiti View sa modelom
            return View("ProductDetails",model);
        }


    }
}