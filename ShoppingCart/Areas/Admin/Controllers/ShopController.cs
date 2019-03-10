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

        // Post: Admin/Shop/AddNewCategory
        [HttpPost]
        //catName dolazi iz Ajax
        public string AddNewCategory(string catName)
        {
            //Deklarisati id
            string id;

            using (ShoppingCartDB db = new ShoppingCartDB())
            {
                //proveriti da li je category ime jedinstveno 
                if (db.Categories.Any(x => x.Name == catName))
                {
                    return "titletaken";
                }
                //inicijalizovati DTO
                CategoriesDTO dto = new CategoriesDTO();
                //Dodati u DTO
                dto.Name = catName;
                dto.Slug = catName.Replace(" ", "-").ToLower();
                //ista logika kao i za pageove kada se doda kategorija bice poslednja
                dto.Sorting = 100;
                //sacuvati DTO
                db.Categories.Add(dto);
                db.SaveChanges();
                //uzeti ubaceni id
                id = dto.Id.ToString();
            }
            //vratiti taj id
            return id;
        }

        //POST: Admin/Shop/ReorderCategories
        [HttpPost]
        public void ReorderCategories(int[] id)
        {
            using (ShoppingCartDB db = new ShoppingCartDB())
            {
                // napraviti brojac
                int br = 1;
                //Deklarisanje CategoriesDTO
                CategoriesDTO dto;
                //Postaviti sorting za svaki page
                foreach (var item in id)
                {
                    //pronadji Categories sa vrednoscu koju trenutno ima item u db
                    dto = db.Categories.Find(item);
                    //postavi soting da bude isti kao i br
                    dto.Sorting = br;
                    //sacuvaj promene u db
                    db.SaveChanges();
                    //inkrementuj brojac
                    br++;
                }

            }
        }

        //Get: Admin/Shop/DeleteCategory/id
        public ActionResult DeleteCategory(int id)
        {

            using (ShoppingCartDB db = new ShoppingCartDB())
            {
                //Pronaci Category sa id-om u bazi
                CategoriesDTO dto = db.Categories.Find(id);
                //Ukloniti taj Category
                db.Categories.Remove(dto);
                //Sacuvati promene u bazi
                db.SaveChanges();
            }
            //Redirektovati na index
            return RedirectToAction("Categories");
        }
    }
}