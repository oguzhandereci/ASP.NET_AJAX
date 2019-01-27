using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ASP.NET_AJAX.Models;
using ASP.NET_AJAX.Models.ViewModels;

namespace ASP.NET_AJAX.Controllers
{
    public class CategoryController : Controller
    {
        // GET: Category
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public JsonResult Search(string s)
        {
            var key = s.ToLower();
            if (key.Length<=2 && key != "*")
            {
                return Json(new ResponseData()
                {
                    message = "Arama yapmak icin 2 karakterden fazla girilmelidir",
                    success = false
                },JsonRequestBehavior.AllowGet);
            }

            try
            {
                var db = new NorthwindEntities();
                List<CategoryViewModel> data;
                if (key == "*")
                {
                    data = db.Categories.OrderBy(x=>x.CategoryName).Select(x => new CategoryViewModel()
                    {
                        CategoryID = x.CategoryID,
                        CategoryName = x.CategoryName,

                        Description = x.Description,
                        ProductCount = x.Products.Count
                    }).ToList();
                }
                else
                {
                    data = db.Categories.OrderBy(x => x.CategoryName)
                                        .Where(x => x.CategoryName.ToLower().Contains(key))
                                        .Select(x => new CategoryViewModel()
                                        {
                                            CategoryID = x.CategoryID,
                                            CategoryName = x.CategoryName,
                                            Description = x.Description,
                                            ProductCount = x.Products.Count
                                        }).ToList();
                }
                return Json(new ResponseData()
                {
                    message = $"{data.Count} kayıtlı kategori bulundu",
                    success = true,
                    data = data
                }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                return Json(new ResponseData()
                {
                    message = $"Arama isleminde hata olustu:{e.Message}",
                    success = false
                }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult Detail(int id)
        {
            return null;
        }

        public JsonResult Delete(int id)
        {
            return null;
        }
    }
}