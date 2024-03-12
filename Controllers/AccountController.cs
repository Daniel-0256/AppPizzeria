using AppPizzeria.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace AppPizzeria.Controllers
{
    public class AccountController : Controller
    {
        private DBContext db = new DBContext();

        // GET: Account/Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var existingUser = db.Users
                    .FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password); // Assicurati che la password sia criptata in produzione!

                if (existingUser != null)
                {
                    FormsAuthentication.SetAuthCookie(model.Email, false);
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home"); // o qualsiasi pagina tu voglia mostrare dopo il login
                    }
                }
                else
                {
                    ModelState.AddModelError("", "L'indirizzo email o la password forniti non sono corretti.");
                }
            }

            return View(model);
        }

        // POST: Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home"); // Reindirizza alla pagina iniziale dopo il logout
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}