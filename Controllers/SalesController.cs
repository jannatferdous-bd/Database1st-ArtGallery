using ArtGallery.Models;
using ArtGallery.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace ArtGallery.Controllers
{
   
    public class SalesController : Controller
    {
        private AppDbContext db = new AppDbContext();

        [HttpGet]
        [Authorize]
        public ActionResult Index(string sortOrder, string currentFilter, string SearchString ,int? page)
        {


            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            
            if (SearchString != null)
            {
                page = 1;
            }
            else
            {
                SearchString = currentFilter;
            }
            ViewBag.CurrentFilter = SearchString;

            var sales = from s in db.Sales
                        select s;
           
         
            if (!String.IsNullOrEmpty(SearchString))
            {
                sales = sales.Where(s =>
               s.CustomerName.Contains(SearchString) ||
               s.CustomerPhone.Contains(SearchString));

               
            }



            switch (sortOrder)
            {
                case "name_desc":
                    sales = sales.OrderByDescending(s => s.CustomerName);
                    break;
                case "Date":
                    sales = sales.OrderBy(s => s.SaleDate);
                    break;
                case "date_desc":
                    sales = sales.OrderByDescending(s => s.SaleDate);
                    break;
                default:
                    sales = sales.OrderBy(s => s.CustomerName);
                    break;
            }
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(sales.ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult CreatePartial()
        {
            SaleViewModel sale = new SaleViewModel();
            sale.PayMethods = db.PayMethods.ToList();

            if (sale.SaleDetails == null)
            {
                sale.SaleDetails = new List<SaleDetail>();
            }

            sale.SaleDetails.Add(new SaleDetail());
            return PartialView("_CreateSalePartial", sale);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public JsonResult CreateSale(SaleViewModel vobj)
        {

         
            if (vobj.SaleDetails == null)
            {
                vobj.SaleDetails = new List<SaleDetail>();
            }


            if (!ModelState.IsValid)
            {
                vobj.PayMethods = db.PayMethods.ToList();
                return Json(new { success = false });
            }

            Sale sale = new Sale
            {
                CustomerName = vobj.CustomerName,
                SaleDate = vobj.SaleDate,
                CustomerPhone = vobj.CustomerPhone,
                PayMethodId = vobj.PayMethodId,
                IsPaid = vobj.IsPaid,
                TotalPrice = vobj.TotalPrice,
                SaleDetails = vobj.SaleDetails
            };

            if (vobj.ProfileFile != null)
            {
                string uniqueFileName = GetFileName(vobj.ProfileFile);
                sale.ImageUrl = uniqueFileName;
            }
            else
            {
                sale.ImageUrl = "noimage.jpg";
            }

            var errorList = ModelState.Values
              .SelectMany(v => v.Errors)
              .Select(e => e.ErrorMessage)
              .ToList();
            ViewBag.ErrorList = errorList;

            db.Sales.Add(sale);
            db.SaveChanges();

            return Json(new { success = true, redirectUrl = Url.Action("Index") });
        }

    

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult EditPartial(int id)
        {
            var sale = db.Sales
                .Include(c => c.PayMethod)
                .Include(c => c.SaleDetails)
                .FirstOrDefault(s => s.SaleId == id);

            if (sale == null)
            {
                return HttpNotFound("Sale Not found");
            }

            var vObj = new SaleViewModel
            {
                CustomerName = sale.CustomerName,
                SaleId = sale.SaleId,
                SaleDate = sale.SaleDate,
                CustomerPhone = sale.CustomerPhone,
                PayMethodId = sale.PayMethodId,
                IsPaid = sale.IsPaid,
                TotalPrice = sale.TotalPrice,
                ImageUrl = sale.ImageUrl,
                SaleDetails = sale.SaleDetails.ToList(),
                PayMethods = db.PayMethods.ToList()
            };

            return PartialView("_EditSalePartial", vObj);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public JsonResult EditSale(SaleViewModel vobj, string OldImageUrl)
        {
            if (vobj.SaleDetails == null)
            {
                vobj.SaleDetails = new List<SaleDetail>();
            }

            if (!ModelState.IsValid)
            {
                vobj.PayMethods = db.PayMethods.ToList();
                return Json(new { success = false });
            }

            Sale obj = db.Sales
                .Include(a => a.SaleDetails)
                .FirstOrDefault(x => x.SaleId == vobj.SaleId);

            if (obj == null)
            {
                return Json(new { success = false, errors = new[] { "Sale not found." } });
            }

            obj.CustomerName = vobj.CustomerName;
            obj.PayMethodId = vobj.PayMethodId;
            obj.CustomerPhone = vobj.CustomerPhone;
            obj.IsPaid = vobj.IsPaid;
            obj.SaleDate = vobj.SaleDate;
            obj.TotalPrice = vobj.TotalPrice;
            obj.ImageUrl = vobj.ProfileFile != null ? GetFileName(vobj.ProfileFile) : OldImageUrl;

            var updatedDetailIds = vobj.SaleDetails
                .Where(m => m.SaleDetailId > 0)
                .Select(m => m.SaleDetailId)
                .ToList();

            var detailsToRemove = obj.SaleDetails
                .Where(m => !updatedDetailIds.Contains(m.SaleDetailId))
                .ToList();

            foreach (var item in detailsToRemove)
            {
                db.SaleDetails.Remove(item);
            }

            foreach (var item in vobj.SaleDetails)
            {
                if (item.SaleDetailId > 0)
                {
                    var existing = obj.SaleDetails.FirstOrDefault(m => m.SaleDetailId == item.SaleDetailId);
                    if (existing != null)
                    {
                        existing.ArtName = item.ArtName;
                        existing.Quantity = item.Quantity;
                    }
                }
                else
                {
                    obj.SaleDetails.Add(new SaleDetail
                    {
                        SaleId = obj.SaleId,
                        ArtName = item.ArtName,
                        Quantity = item.Quantity
                    });
                }
            }

            db.SaveChanges();
            return Json(new { success = true, redirectUrl = Url.Action("Index") });
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public JsonResult DeleteSale(int id)
        {
            Sale sale = db.Sales.Find(id);

            if (sale == null)
            {
                return Json(new { success = false, message = "Sale not found." });
            }

            var details = db.SaleDetails.Where(s => s.SaleId == id).ToList();
            db.SaleDetails.RemoveRange(details);
            db.Sales.Remove(sale);
            db.SaveChanges();

            return Json(new { success = true, redirectUrl = Url.Action("Index") });
        }

        private string GetFileName(HttpPostedFileBase file)
        {
            string fileName = "";
            if (file != null)
            {
                fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                string path = Path.Combine(Server.MapPath("~/images/"), fileName);
                file.SaveAs(path);
            }
            return fileName;
        }
    }
}