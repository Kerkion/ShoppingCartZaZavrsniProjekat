using ShoppingCart.Models.Data;
using ShoppingCart.Models.ViewModels.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShoppingCart.Areas.Admin.Controllers
{
    public class PagesController : Controller
    {
        // GET: Admin/Pages
        public ActionResult Index()
        {
            //Deklarisacemo listu PageVM
            List<PageVM> pagesList;
            //Korisitmo using iz razloga zato sto kad zavrsi sve ocisti za sobom(zatvori konekciju i sl.) moze i bez using ali ovako je sigurnije,pravimo novu istancu ShoppingCartDB
            using (ShoppingCartDB db = new ShoppingCartDB())
            {
                //Inicijalizacija liste
                //prvo pristupamo otvorenoj konekciji db,odatle biramo pages,prebacujemo sve to u array,zatim radimo order by po sortingu koji je(po defaultu je DESC(opadajuci),zatim odatle pristupamo PageVM u koji prosledjujemo PageDTO(jedan od razloga zasto smo pravili ustvari konstruktor u pageVM-u) i to sve prebacujemo u listu)
                //Tako cemo dobiti narodski receno spisak svih stranica
                pagesList = db.Pages.ToArray().OrderBy(x => x.Sorting).Select(x => new PageVM(x)).ToList();
            }
            //Vratit View sa napravljenom listom
            return View(pagesList);

            //na kraju na desni klik bilo gde unutar ovog controlera isli smo na AddView(ostavili ime index,za template smo izabrali listu,i posle toga za model dodali PagesVM) i napravili novi view za PageController
        }

        //preusmeravanje za dodavanje stranice
        //GET : Admin/Pages/AddPage
        [HttpGet]
        public ActionResult AddPAge()
        {
            return View();
        }

        //POST : Admin/Pages/AddPage
        [HttpPost]
        public ActionResult AddPAge(PageVM model)
        {
            //Proveriti model
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (ShoppingCartDB db = new ShoppingCartDB())
            {
                //Deklarisati Slug
                string slug;
                //Inicijalizacija DTO(Data transfer Object) PageDTO
                PageDTO dto = new PageDTO();
                //Koristiti da se doda vrednost za title u DTO
                dto.Title = model.Title;
                //Proveriti i dodati Slug ako je potrebno
                if (string.IsNullOrWhiteSpace(model.Slug))
                {
                    //Ukoliko je ostavljen white space ili nepostoji uzecemo title zameniti white space sa - i prebaciti u mala slova
                    slug = model.Title.Replace(" ", "-").ToLower();
                }
                else
                {
                    //Ukoliko ima nesto napiosano opet cemo prtazna polja zameniti sa - i prebaciti u mala slova
                    slug = model.Slug.Replace(" ", "-").ToLower();
                }
                //Pobrinuti se da su Title i Slug unikatni
                if (db.Pages.Any(x => x.Title == model.Title || db.Pages.Any(s => s.Slug == slug)))
                {
                    ModelState.AddModelError("", "That title or a slug already exists!");
                    return View(model);
                }

                //Popuniti ostatak DTO-a
                dto.Slug = slug;
                dto.Body = model.Body;
                dto.HasSidebar = model.HasSidebar;
                //Ideja je da kada se doda nova stranica uvek bude zadnja(radunamo da nece biti vise od sto stranica napravljeno u isto vreme)
                dto.Sorting = 100;
                //Sacuvati DTO
                db.Pages.Add(dto);
                //Sacuvati u bazi podataka
                db.SaveChanges();
            }

            //Sacuvati privremenu poruku koja ostaje i posle requesta(za razliku od viewbage koji je bas privremen nestaje posle requesta),ovde koristimo da bi ostao i da bi smo mogli da ga dodamo u view
            TempData["SM"] = "You succesfully added a new page";
            //Redirektiovati na add page koji je onaj gore get
            return RedirectToAction("AddPAge");
        }

        // GET: Admin/Pages/EditPage/id
        [HttpGet]
        public ActionResult EditPage(int id)
        {
            //Deklarisanje PageVM-a
            PageVM page;

            using (ShoppingCartDB db = new ShoppingCartDB())
            {
                //GET(Uzmi) Stranicu
                PageDTO dto = db.Pages.Find(id);
                //Potvrditi da postoji takva stranica
                if (dto == null)
                {
                    return Content("The Page doesn't exist!!!");
                }
                //Inicijalizzacija PageVm-a 
                page = new PageVM(dto);
            }
            //Vratit View sa modelom
            return View(page);
        }

        //POST: Admin/Pages/Edit/id
        [HttpPost]
        public ActionResult EditPage(PageVM model)
        {
            //Proveriti da li postoji model
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (ShoppingCartDB db = new ShoppingCartDB())
            {
                //Pronaci page id
                int id = model.Id;
                //Inicijalizovati slug
                string slug = "home";
                //Pronaci page
                PageDTO page = db.Pages.Find(id);
                //DTO title
                page.Title = model.Title;
                //Proveriti da li je popunjen slug i postaviti ga ukoliko je potrebno
                if (model.Slug != "home")
                {
                    if (string.IsNullOrWhiteSpace(model.Slug))
                    {
                        slug = model.Title.Replace(" ", "-").ToLower();
                    }
                    else
                    {
                        slug = model.Slug.Replace(" ", "-").ToLower();
                    }
                }
                //Proveriti da li su Title i Slug unikatni
                if (db.Pages.Where(x => x.Id != id).Any(x => x.Title == model.Title) || 
                    db.Pages.Where(x => x.Id != id).Any(x => x.Slug == slug))
                {
                    ModelState.AddModelError("", "Title or Slug already exists!!!");
                    return View(model);
                }
                //DTO ostatak
                page.Slug = slug;
                page.Body = model.Body;
                page.HasSidebar = model.HasSidebar;

                //Sacuvati DTO
                db.SaveChanges();

            }
            //Postaviti TempData poruku
            TempData["SM"] = "You have edited page!";
            
            //Redirektovati
            return RedirectToAction("EditPage");
        }

        //Get : Admin/Pages/PageDetails/id
        public ActionResult PageDetails(int id)
        {
            //Deklarisanje PageVM
            PageVM page;
            using(ShoppingCartDB db = new ShoppingCartDB())
            {
                //pronaci(get) stranicu
                PageDTO dto = db.Pages.Find(id);

                //potvrditi da postoji takva stranica
                if(dto == null)
                {
                    return Content("That page doesn't exist!!!");
                }
                //Inicijalizacija PageVM
                page = new PageVM(dto);

            }
            return View(page);
        }

        //Get: Admin/Pages/DeletePage/id
        public ActionResult DeletePage(int id)
        {
            
            using(ShoppingCartDB db = new ShoppingCartDB())
            {
                //Pronaci page sa id-om u bazi
                PageDTO dto = db.Pages.Find(id);
                //Ukloniti taj page
                db.Pages.Remove(dto);
                //Sacuvati promene u bazi
                db.SaveChanges();
            }
            //Redirektovati na index
            return RedirectToAction("Index");
        }

        //POST: Admin/Pages/ReorderPages
        [HttpPost]
        public void ReorderPages(int[] id)
        {
            using(ShoppingCartDB db = new ShoppingCartDB())
            {
                // napraviti brojac
                int br = 1;
                //Deklarisanje PageDTO
                PageDTO dto;
                //Postaviti sorting za svaki page
                foreach (var item in id)
                {
                    //pronadji page sa vrednoscu koju trenutno ima item u db
                    dto = db.Pages.Find(item);
                    //postavi soting da bude isti kao i br
                    dto.Sorting = br;
                    //sacuvaj promene u db
                    db.SaveChanges();
                    //inkrementuj brojac
                    br++;
                }

            }
        }

        //Get: Admin/Pages/EditSidebar
        [HttpGet]
        public ActionResult EditSidebar()
        {
            //Deklarisanje modela
            SidebarVM sidebar;

            using (ShoppingCartDB db = new ShoppingCartDB())
            {
                //Uzimanje DTO
                SidebarDTO dto = db.Sidebar.Find(1);
                //Inicijalizacija modela
                sidebar = new SidebarVM(dto);
            }
            //vratiti View sa modelom(sidebar)
            return View(sidebar);
        }

        //POST: Admin/Pages/EditSidebar
        [HttpPost]
        public ActionResult EditSidebar(SidebarVM model)
        {
            using(ShoppingCartDB db = new ShoppingCartDB())
            {
                //Uzmi DTO
                SidebarDTO dto = db.Sidebar.Find(1);
                //DTO body
                dto.Body = model.Body;
                //Sacuvaj
                db.SaveChanges();
            }
            //Postavi TempData poruku
            TempData["SM"] = "You succesfully edited a sidebar!!!";
            //Redirect
            return RedirectToAction("EditSidebar");
        }
    }
}