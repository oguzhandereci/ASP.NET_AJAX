using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ASP.NET_AJAX.Models;
using ASP.NET_AJAX.Models.ViewModels;

namespace ASP.NET_AJAX.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetAllCategories()
        {
            try
            {
                var db = new NorthwindEntities();
                var data = db.Categories.OrderBy(x => x.CategoryName).Select(x => new CategoryViewModel()
                {
                    CategoryID = x.CategoryID,
                    CategoryName = x.CategoryName,
                    ProductCount = x.Products.Count
                }).ToList();

                return Json(new ResponseData()
                {
                    data = data,
                    success = true,
                    message = $"{data.Count} adet kayıtlı kategori bulunmaktadır"
                },JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new ResponseData()
                {
                    success = false,
                    message = $"Bir hata olustu : {ex.Message}"
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetAllProducts()
        {
            try
            {
                var db = new NorthwindEntities();
                var data = db.Products.OrderBy(x => x.ProductName)
                                      .ToList()
                                      .Select(x => new ProductViewModel()
                {
                    CategoryName = x.Category?.CategoryName,
                    CategoryID = x.CategoryID,
                    ProductName = x.ProductName,
                    UnitsInStock = x.UnitsInStock,
                    UnitPrice = x.UnitPrice,
                    ProductID = x.ProductID,
                    Discontinued = x.Discontinued,
                    QuantityPerUnit = x.QuantityPerUnit,
                    ReorderLevel = x.ReorderLevel,
                    SupplierID = x.SupplierID,
                    SupplierName = x.Supplier?.CompanyName,
                    UnitPriceFormatted = $"{x.UnitPrice:c2}",
                    UnitsOnOrder = x.UnitsOnOrder
                }).ToList();

                return Json(new ResponseData()
                {
                    data = data,
                    message = $"{data.Count} adet urun bulunmaktadir",
                    success = true,
                    responseTime = DateTime.Now
                },JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new ResponseData()
                {
                    message = $"Bir hata olustu: {ex.Message}",
                    success = false,
                    responseTime = DateTime.Now
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            try
            {
                var db = new NorthwindEntities();
                var data = db.Products.Find(id);
                db.Products.Remove(data ?? throw new InvalidOperationException());
                db.SaveChanges();

                return Json(new ResponseData()
                {
                    data = data,
                    success = true,
                    message = $"{data.ProductName} isimli ürün başarıyla silinmiştir."
                },JsonRequestBehavior.AllowGet);
            }
            catch (InvalidOperationException e)
            {
                return Json(new ResponseData()
                {
                    message = $"Bir hata olustu : {e.Message}",
                    success = false
                },JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new ResponseData()
                {
                    message = $"Bir hata olustu : {ex.Message}",
                    success = false
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Update(Product prd)
        {
            try
            {
                var db = new NorthwindEntities();
                var data = db.Products.Find(prd.ProductID);
                data.ProductName = prd.ProductName;
                data.UnitPrice = prd.UnitPrice;
                data.CategoryID = prd.CategoryID;
                data.QuantityPerUnit = prd.QuantityPerUnit;
                db.SaveChanges();

                return Json(new ResponseData()
                {
                    message = $"{data.ProductName} isimli ürün başarıyla güncellenmiştir.",
                    success = true
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new ResponseData()
                {
                    success = false,
                    message = $"Güncelleme işlemi sırasında bir hata oluştu: {e.Message}"
                }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}