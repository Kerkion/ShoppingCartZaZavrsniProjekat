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
    }
}