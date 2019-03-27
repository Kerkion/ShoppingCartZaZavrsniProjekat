using ShoppingCart.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ShoppingCart
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_AuthenticateRequest()
        {
            //proveriti da li je korisnik logovan
            if(User == null)
            {
                return;
            }

            //pronaci username
            string username = Context.User.Identity.Name;

            //deklarisati array rols
            string[] roles = null;
            using (ShoppingCartDB db = new ShoppingCartDB())
            {
                //napuniti array roles
                UserDTO dto = db.Users.FirstOrDefault(x => x.Username == username);
                //izabrati koji je naziv rola
                roles = db.UserRoles.Where(x => x.UserId == dto.Id).Select(x => x.Role.Roles).ToArray();
                
            }
            //izgraditi IPrincipal obj
            IIdentity userIdentity = new GenericIdentity(username);

            IPrincipal newUserObject = new GenericPrincipal(userIdentity, roles);

            //Update-ovati Context.User
            Context.User = newUserObject;
        }
    }
}
