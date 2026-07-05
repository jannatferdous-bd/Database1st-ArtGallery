using ArtGallery.Models;
using ArtGallery.ViewModels;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace ArtGallery.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdministratorController : Controller
    {
        private AppDbContext db = new AppDbContext();

        [HttpGet]
        public ActionResult RoleCreate()
        {
            var model = new UserRoleViewModel();
            model.UserList = db.tblUsers.ToList();

            ViewBag.UserId = new SelectList(model.UserList, "Id", "UserName");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RoleCreate(UserRoleViewModel vobj)
        {
            vobj.UserList = db.tblUsers.ToList();
            ViewBag.UserId = new SelectList(vobj.UserList, "Id", "UserName", vobj.UserId);

            if (!ModelState.IsValid)
            {
                return View(vobj);
            }

            var userExists = db.tblUsers.Any(u => u.Id == vobj.UserId);
            if (!userExists)
            {
                ModelState.AddModelError("UserId", "Selected user does not exist");
                return View(vobj);
            }

            var roleExists = db.tblRoles.Any(r => r.UserId == vobj.UserId && r.RoleName == vobj.RoleName);
            if (roleExists)
            {
                ModelState.AddModelError("", "This user already has this role");
                return View(vobj);
            }

            tblRole role = new tblRole
            {
                RoleName = vobj.RoleName,
                UserId = vobj.UserId
            };

            db.tblRoles.Add(role);
            db.SaveChanges();

            return RedirectToAction("Index", "Account");
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