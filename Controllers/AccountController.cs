using ArtGallery.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace ArtGallery.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private AppDbContext db = new AppDbContext();

        // GET: Account
        [Authorize]
        [HttpGet]
        public ActionResult Index()
        {
            IEnumerable<tblUser> list = db.tblUsers.ToList();
            return View(list);
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(tblUser user)
        {

           
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            var validUser = db.tblUsers
                .FirstOrDefault(u => u.UserName == user.UserName && u.Password == user.Password);
            if (validUser != null)
            {
                var role = db.tblRoles.FirstOrDefault(r => r.UserId == validUser.Id);

                string roleName = role != null ? role.RoleName : "User";

                FormsAuthentication.SetAuthCookie(validUser.UserName, false);
                Session["Role"] = roleName;

                return RedirectToAction("Index", "Sales");
            }
           

            ViewBag.Error = "Invalid username or password";
            return View(user);
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(tblUser user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            bool exists = db.tblUsers.Any(u => u.UserName == user.UserName);

            if (exists)
            {
                ModelState.AddModelError("UserName", "Username already exists");
                return View(user);
            }

            tblUser newUser = new tblUser
            {
                UserName = user.UserName,
                Password = user.Password
            };

            db.tblUsers.Add(newUser);
            db.SaveChanges();

            // First user becomes Admin
            if (db.tblUsers.Count() == 1)
            {
                tblRole role = new tblRole
                {
                    UserId = newUser.Id,
                    RoleName = "Admin"
                };

                db.tblRoles.Add(role);
                db.SaveChanges();
            }

            return RedirectToAction("Login", "Account");
        }

        [Authorize]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var user = db.tblUsers.Find(id);

            if (user == null)
                return HttpNotFound();

            var role = db.tblRoles.FirstOrDefault(r => r.UserId == id);
            ViewBag.RoleName = role != null ? role.RoleName : "User";

            return View(user);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tblUser user, string RoleName)
        {
            var existingUser = db.tblUsers.Find(user.Id);

            if (existingUser == null)
                return HttpNotFound();

            existingUser.UserName = user.UserName;
            existingUser.Password = user.Password;

            // role update
            var role = db.tblRoles.FirstOrDefault(r => r.UserId == user.Id);

            if (role != null)
                role.RoleName = RoleName;
            else
                db.tblRoles.Add(new tblRole { UserId = user.Id, RoleName = RoleName });

            db.SaveChanges();

            return RedirectToAction("Index");
        }
        [Authorize]
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var user = db.tblUsers.Find(id);

            if (user == null)
                return HttpNotFound();

            return View(user);
        }
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var user = db.tblUsers.Find(id);

            if (user == null)
                return HttpNotFound();

            // delete role first
            var role = db.tblRoles.FirstOrDefault(r => r.UserId == id);
            if (role != null)
                db.tblRoles.Remove(role);

            db.tblUsers.Remove(user);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
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