using PagedList;
using ShoppingCart.Models.Data;
using ShoppingCart.Models.ViewModels.Pages;
using ShoppingCart.Models.ViewModels.Shop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
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

        //POST: Admin/Shop/RenameCategory
        [HttpPost]
        public string RenameCategory(string newCatName, int id)
        {
            using (ShoppingCartDB db = new ShoppingCartDB())
            {
                //proveriti da li je ime kategorije unikatno
                if (db.Categories.Any(x => x.Name == newCatName)) {
                    return "titletaken";
                }
                //Uzeti DTO
                CategoriesDTO dto = db.Categories.Find(id);
                //Prepraviti DTO
                dto.Name = newCatName;
                dto.Slug = newCatName.Replace(" ", "-").ToLower();
                //Sacuvati
                db.SaveChanges();
            }
            //Vratiti
            return "done";
        }

        //Get: Admin/Shop/AddProducts
        [HttpGet]
        public ActionResult AddProducts()
        {
            //Inicijalizuj model
            ProductsVM model = new ProductsVM();
            //Dodaj selectovanu listu kategorija modelu
            using (ShoppingCartDB db = new ShoppingCartDB())
            {
                model.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");
            }
            //Vratiti view sa modelom 
            return View(model);
        }

        //Post: Admin/Shop/AddProducts
        [HttpPost]
        public ActionResult AddProducts(ProductsVM model,HttpPostedFileBase file)
        {
            //proveriti stanje modela
            if (!ModelState.IsValid)
            {
                //Zbog slect liste morace da se popuni svaki put pre nego sto se vrati view
                using (ShoppingCartDB db = new ShoppingCartDB())
                {
                    model.Categories = new SelectList(db.Categories.ToList(),"Id","Name");
                    return View(model);
                }
                
            }
            using (ShoppingCartDB db = new ShoppingCartDB())
            {
                //proveriti da li je Name product-a unikatno
                if (db.Products.Any(x => x.Name == model.Name))
                {
                    model.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");
                    ModelState.AddModelError("", "That product name already exists!!!");
                    return View(model);
                }
            }
                
             //Deklarisati product id
             int id;
             //Inicijalizovati i sacuvati ProductsDTO
            using (ShoppingCartDB db = new ShoppingCartDB())
            {
                ProductsDTO product = new ProductsDTO();
                product.Name = model.Name;
                product.Slug = model.Name.Replace(" ", "-").ToLower();
                product.Description = model.Description;
                product.Price = model.Price;
                product.CategoryId = model.CategoryId;

                //Pronaci ime koje je izbrano iz Category
                CategoriesDTO category = db.Categories.FirstOrDefault(x => x.Id == model.CategoryId);
                product.CategoryName = category.Name;

                db.Products.Add(product);
                db.SaveChanges();
                //Uzeti ubaceni Id
                id = product.Id;
            }

            //Postaviti TempData poruku(postavljamo sad u slucaju da korisnik pokusa da ubaci text file ili neku drugu vrstu file-a da obavestimo da je dodato sve osim slike )
            TempData["SM"] = "You have added a new product!";

            #region Upload slike
            //napraviti direktorijume za cuvanje slika
            var rootDirectory = new DirectoryInfo(string.Format("{0}Images\\Uploads",Server.MapPath(@"\")));
            //putanja za products folder
            var pathStringProducts = Path.Combine(rootDirectory.ToString(), "Products");
            //putanja za produt id folder
            var pathStringProductsId = Path.Combine(rootDirectory.ToString(), "Products\\" + id.ToString());
            //putanja za product thumbnail folder
            var pathStringProductsTumb = Path.Combine(rootDirectory.ToString(), "Products\\" + id.ToString() + "\\Thumbs");
            //putanja za Galeriju folder
            var pathStringProductsGallery = Path.Combine(rootDirectory.ToString(), "Products\\" + id.ToString() + "\\Gallery");
            //putanja za galery thumbove folder
            var pathStringProductsGalleryThumbs = Path.Combine(rootDirectory.ToString(), "Products\\" + id.ToString() + "\\Gallery\\Thumbs");

            //ukoliko ne posto je stvoriti ih(stvorice se samo ukoliko se prvi put dodaje slika)
            if (!Directory.Exists(pathStringProducts))
            {
                Directory.CreateDirectory(pathStringProducts);
            }
            if (!Directory.Exists(pathStringProductsId))
            {
                Directory.CreateDirectory(pathStringProductsId);
            }
            if (!Directory.Exists(pathStringProductsTumb))
            {
                Directory.CreateDirectory(pathStringProductsTumb);
            }
            if (!Directory.Exists(pathStringProductsGallery))
            {
                Directory.CreateDirectory(pathStringProductsGallery);
            }
            if (!Directory.Exists(pathStringProductsGalleryThumbs))
            {
                Directory.CreateDirectory(pathStringProductsGalleryThumbs);
            }
            
            //proveriti da li je file uplodovan

            if(file != null && file.ContentLength > 0)
            {
                //uzeti file extension
                string extension = file.ContentType.ToLower();
                //verifikovati extenziju
                if (extension != "image/jpg" && extension != "image/jpeg" && extension != "image/pjpeg" && extension != "image/gif" && extension != "image/x-png" && extension != "image/png")
                {
                    using (ShoppingCartDB db = new ShoppingCartDB())
                    {
                        model.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");
                        ModelState.AddModelError("", "That format is not supported,the image was not uploaded!!!");
                        return View(model);
                    }
                }
                //inicijalizovati ime slike
                string imgName = file.FileName;
                //Sacuvati ime slike u DTO
                using (ShoppingCartDB db = new ShoppingCartDB())
                {
                    ProductsDTO dto = db.Products.Find(id);
                    dto.ImageName = imgName;

                    db.SaveChanges();
                }
                //postaviit putanje za Orginalnu sliku i tumb sliku
                var path = string.Format("{0}\\{1}", pathStringProductsId, imgName);
                var path1 = string.Format("{0}\\{1}", pathStringProductsTumb, imgName);
                //Sacuvati orginalnu sliku
                file.SaveAs(path);
                //Napraviti i sacuvati tumb
                WebImage img = new WebImage(file.InputStream);
                img.Resize(200, 200);
                img.Save(path1);

            }
            #endregion

            //Redirekt
            return RedirectToAction("AddProducts");
        }

        //Get: Admin/Shop/Products
        public ActionResult Products(int? page,int? catId)
        {
            //Deklarisati listu ProductVM
            List <ProductsVM> listOfProductVM;
            //Postaviti broj stranice
            //Ukoliko nije ni jedna stranica prosla kroz query postavi kao prvu stranicu 
            var pageNumber = page ?? 1;

            using (ShoppingCartDB db = new ShoppingCartDB())
            {
                //Inicijalizovati listu
                listOfProductVM = db.Products.ToArray().Where(x => catId == null || catId == 0 || x.CategoryId == catId).Select(x => new ProductsVM(x)).ToList();
                //Napuniti categories slelected list
                //koristicemo ViewBag posto necemo prikazivati svaki model u listi
                ViewBag.Categories = new SelectList(db.Categories.ToList(),"Id","Name");

                //postaviti izabranu kategoriju
                ViewBag.SelectedCategory = catId.ToString();
            }

            //postaviti obelezavanje stranica
            //prikazivace 25 itema po stranici
            var onePageOfProducts = listOfProductVM.ToPagedList(pageNumber, 2);

            ViewBag.OnePageOfProducts = onePageOfProducts;


            //vratiti View sa modelom
            return View(listOfProductVM);
        }
    }
}