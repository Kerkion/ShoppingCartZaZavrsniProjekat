using ShoppingCart.Models.Data;
using ShoppingCart.Models.ViewModels.Cart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShoppingCart.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        public ActionResult Index()
        {
            //Inicijalizovati listu CartVM
            var cart = Session["cart"] as List<CartVM> ?? new List<CartVM>();
            //Proveriti da li je cart prazan
            if(cart.Count == 0 || Session["cart"] == null) {
                ViewBag.Msg = "Your cart is empty";
                return View();
            }
            //Izracunati koliki je total i sacuvati u ViewBag
            decimal total = 0m;

            foreach (var item in cart)
            {
                total += item.Total; 
            }

            ViewBag.Total = total;
            //vratiti View sa listom
            return View(cart);
        }

        //partial view za cart
        public ActionResult CartPartialView()
        {
            //Inicijalizovati CartVM
            CartVM model = new CartVM();
            //Inicijalizovati kolicinu(Quantity)
            int quantity = 0;
            //Inicijalizacija cene(Price)
            decimal price = 0m;
            //Proveriti da li postoji session cart
            if(Session["cart"] != null)
            {
                //Pronaci kolika je ukupna vrednost(Total) i cena (Price)
                var list = (List<CartVM>)Session["cart"];

                foreach (var item in list)
                {
                    quantity += item.Quantity;
                    price += item.Price * item.Quantity;
                }
                model.Quantity = quantity;
                model.Price = price;
            }
            else
            {
                // postaviti quantity and price na 0
                model.Quantity = 0;
                model.Price = 0m;
            }

            //vratiti partial view sa modelom
            return PartialView(model);
        }

        //partial view za dodavanje u cart
        public ActionResult AddToCartPartialView(int id)
        {
            //inicijalizovati listu CartVM
            List<CartVM> cartVmList = Session["cart"] as List<CartVM> ?? new List<CartVM>();
            //inicijalizovati model CartVM
            CartVM model = new CartVM();

            using (ShoppingCartDB db = new ShoppingCartDB())
            {
                //uzeti product
                ProductsDTO dto = db.Products.Find(id);
                //proveriti da li se product nalazi vec u cart-u
                var productInCart = cartVmList.FirstOrDefault(x => x.ProductId == id);
                if(productInCart == null)
                {
                    //Ukoliko se ne nalazi dodati novi
                    cartVmList.Add(new CartVM()
                    {
                        ProductId = dto.Id,
                        ProductName = dto.Name,
                        Price = dto.Price,
                        Quantity = 1,
                        Image = dto.ImageName
                    });
                }
                else
                {
                    //Ukoliko postoji incrementovati
                    productInCart.Quantity++;
                }

            }

            //pronaci total i dodati u model
            int qty = 0;
            decimal price = 0m;

            foreach (var item in cartVmList)
            {
                qty += item.Quantity;
                price += item.Price * item.Quantity;
            }
            model.Quantity = qty;
            model.Price = price;
            //sacuvati cart nazad u session
            Session["cart"] = cartVmList;
            //vratiti partial view sa modelom
            return PartialView(model);
        }

        //Get: cart/IncrementProduct
        public JsonResult IncrementProduct(int productId)
        {
            //inicijalizovati cart listu
            List<CartVM> listCart = Session["cart"] as List<CartVM>;

            using (ShoppingCartDB db = new ShoppingCartDB())
            {
                //pronaci cartVm koristeci productId
                CartVM model = listCart.FirstOrDefault(x => x.ProductId == productId);
                //incrementovati kolicinu
                model.Quantity++;
                //sacuvati quantity i price
                var resault = new { quantity = model.Quantity, price = model.Price};
                //vratiti json sa podacima
                return Json(resault,JsonRequestBehavior.AllowGet);
            }
        }

        //Get: cart/DecrementProduct
        public JsonResult DecrementProduct(int productId)
        {
            //inicijalizovati cart listu
            List<CartVM> listCart = Session["cart"] as List<CartVM>;

            using (ShoppingCartDB db = new ShoppingCartDB())
            {
                //pronaci cartVm koristeci productId
                CartVM model = listCart.FirstOrDefault(x => x.ProductId == productId);
                //dekrementovati kolicinu
                if(model.Quantity > 1)
                {
                    model.Quantity--;
                }
                else
                {
                    model.Quantity = 0;
                    listCart.Remove(model);
                }
                
                //sacuvati quantity i price
                var resault = new { quantity = model.Quantity, price = model.Price };
                //vratiti json sa podacima
                return Json(resault, JsonRequestBehavior.AllowGet);
            }
        }
        //Get: cart/RemoveProduct
        public void RemoveProduct(int productId)
        {
            //inicijalizovati cart listu
            List<CartVM> listCart = Session["cart"] as List<CartVM>;

            using (ShoppingCartDB db = new ShoppingCartDB())
            {
                //pronaci cartVm koristeci productId
                CartVM model = listCart.FirstOrDefault(x => x.ProductId == productId);
                //Ukloniti iz liste model
                listCart.Remove(model);
            }

        }
    }
}