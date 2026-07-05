using ArtGallery.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ArtGallery.Controllers
{
    public class PayMethodsController : Controller
    {
        private AppDbContext db = new AppDbContext();
        // GET: PayMethods
        public ActionResult Index()
        {
            return View(db.PayMethods.ToList());
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PayMethod payMethod = db.PayMethods.Find(id);
            if (payMethod == null)
            {
                return HttpNotFound();
            }
            return View(payMethod);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PayMethodId,PayMethodName")] PayMethod payMethod)
        {
            if (ModelState.IsValid)
            {
                db.PayMethods.Add(payMethod);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(payMethod);
        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PayMethod payMethod = db.PayMethods.Find(id);
            if (payMethod == null)
            {
                return HttpNotFound();
            }
            return View(payMethod);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PayMethodId,PayMethodName")] PayMethod payMethod)
        {
            if (ModelState.IsValid)
            {
                db.Entry(payMethod).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(payMethod);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PayMethod payMethod = db.PayMethods.Find(id);
            if (payMethod == null)
            {
                return HttpNotFound();
            }
            return View(payMethod);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PayMethod payMethod = db.PayMethods.Find(id);
            db.PayMethods.Remove(payMethod);
            db.SaveChanges();
            return RedirectToAction("Index");
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