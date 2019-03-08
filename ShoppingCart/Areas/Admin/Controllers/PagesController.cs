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
    }
}