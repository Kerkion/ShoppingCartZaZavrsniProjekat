using ShoppingCart.Models.Account;
using ShoppingCart.Models.Data;
using ShoppingCart.Models.ViewModels.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ShoppingCart.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            return Redirect("~/account/login");
        }

        // GET: /account/create-account
        [ActionName("create-account")]
        [HttpGet]
        public ActionResult CreateAccount()
        {
            return View("CreateAccount");
        }

        // Post: /account/create-account
        [ActionName("create-account")]
        [HttpPost]
        public ActionResult CreateAccount(UserVM model)
        {
            //proveriti stanje modela
            if (!ModelState.IsValid)
            {
                return View("CreateAccount",model);
            }
            //proveriti da li se slazu passwordi
            if (!model.Password.Equals(model.ConfirmPassword))
            {
                ModelState.AddModelError("", "Password and Confirm Password doesn't match!");
                return View("CreateAccount", model);
            }
            using (ShoppingCartDB db = new ShoppingCartDB())
            {
                //proveriti da li je username unikatan
                if(db.Users.Any(x => x.Username.Equals(model.Username)))
                {
                    ModelState.AddModelError("", "Username is taken");
                    model.Username = "";
                    return View("CreateAccount", model);
                }
                //napraviti userDTO
                UserDTO dto = new UserDTO()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Username = model.Username,
                    Password = model.Password
                };
                //dodati dto
                db.Users.Add(dto);
                //sacuvati
                db.SaveChanges();
                //dodati u UserRolesDTO
                int id = dto.Id;

                UserRolesDTO userRolesDTO = new UserRolesDTO()
                {
                    UserId = id,
                    RoleID = 2
                };
                db.UserRoles.Add(userRolesDTO);
                db.SaveChanges();
            }
            //napraviti temp poruku
            TempData["SM"] = "You succesfully registrated!";
            //redirektovati
            return Redirect("~/account/login");
        }

        // GET: /account/login
        [HttpGet]
        public ActionResult Login()
        {
            //potvrditi da korisnik vec nije ulogovan

            string username = User.Identity.Name;

            if (!string.IsNullOrEmpty(username))
            {
                return RedirectToAction("user-profile");
            }

            //vratiti view
            return View();
        }

        // Post: /account/login
        [HttpPost]
        public ActionResult Login(LoginUserVM model)
        {
            //Proveriti model state
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            //proveriti da li je korisnik validan
            bool isValidUser = false;

            using (ShoppingCartDB db = new ShoppingCartDB())
            {
                if(db.Users.Any(x => x.Username.Equals(model.Username) && x.Password.Equals(model.Password))){
                    isValidUser = true;
                }
            }

            if (!isValidUser)
            {
                ModelState.AddModelError("", "Username or password is incorect!");
                return View(model);
            }
            else
            {
                //pravljenje cookia(Session)
                FormsAuthentication.SetAuthCookie(model.Username, model.Remember);
                return Redirect(FormsAuthentication.GetRedirectUrl(model.Username, model.Remember));
            }
        }

        // Post: /account/Logout
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return Redirect("~/account/login");

        }

        //partial view za prikazivanje imena i prezimena ulogovanog korisinika
        public ActionResult UserPartialNav()
        {
            //pronadji username
            string username = User.Identity.Name;
            //deklarisati model
            NavbarUserPartialVM model;
            using (ShoppingCartDB db = new ShoppingCartDB())
            {
                //pronaci korisnika
                UserDTO dto = db.Users.FirstOrDefault(x => x.Username == username);
                //Izgraditi model
                model = new NavbarUserPartialVM()
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName
                };
            }
            //Vratiti parcijalni view sa modelom 
            return PartialView(model);
        }

        // GET: /account/user-profile
        [HttpGet]
        [ActionName("user-profile")]
        public ActionResult UserProfile()
        {
            //proncai username
            string username = User.Identity.Name;

            //deklarisati model
            UserProfileVM model;
            using (ShoppingCartDB db = new ShoppingCartDB())
            {
                //pronaci korisnika
                UserDTO dto = db.Users.FirstOrDefault(x => x.Username == username);

                //izgraditi model
                model = new UserProfileVM(dto);
            }

            //vratiti View sa modelom
            return View("UserProfile", model);
        }

        // post: /account/user-profile
        [HttpPost]
        [ActionName("user-profile")]
        public ActionResult UserProfile(UserProfileVM model)
        {
            //Proveriti stanje modela
            if (!ModelState.IsValid)
            {
                return View("UserProfile", model);
            }

            //proveriti da lis e passwordi podudaraju

            if (!string.IsNullOrEmpty(model.Password))
            {
                if (!model.Password.Equals(model.ConfirmPassword))
                {
                    ModelState.AddModelError("", "Password and Confirm Password fields doesn't match");
                    return View("UserProfile", model);
                }
            }

            

            using (ShoppingCartDB db = new ShoppingCartDB())
            {
                //pronaci username
                string username = User.Identity.Name;
                //proveriti da li je jedinstven username
                if (db.Users.Where(x => x.Id != model.Id).Any(x => x.Username == username))
                {
                    ModelState.AddModelError("", "That Username is taken!");
                    model.Username = "";
                    return View("UserProfile", model);
                }

                //Editovati DTO
                UserDTO dto = db.Users.Find(model.Id);
                dto.FirstName = model.FirstName;
                dto.LastName = model.LastName;
                dto.Username = model.Username;
                dto.Email = model.Email;

                if (!string.IsNullOrEmpty(model.Password))
                {
                    dto.Password = model.Password;
                }

                //Sacuvati DTO
                db.SaveChanges();
            }
            //postaviti TempData poruku
            TempData["SM"] = "You successfully edited your profile";
            //Redirektovati
            return Redirect("~/account/user-profile");
        }
    }
}