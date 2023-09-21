using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models;
using WebApp.Models.ViewModels;

namespace WebApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly IWebHostEnvironment webHostEnvironment;
        public ProductController(ApplicationDbContext _db, IWebHostEnvironment _webHostEnvironment)
        {
            db = _db;
            webHostEnvironment = _webHostEnvironment;
        }
        public IActionResult Index()
        {
            IEnumerable<Product> objList = db.Products;
            foreach (var obj in objList)
            {
                obj.Category = db.Categories.FirstOrDefault(x => x.CategoryId == obj.CategoryId);
                //obj.ApplicationType = db.ApplicationTypes.FirstOrDefault(x => x.ApplicationTypeId == obj.ApplicationTypeId);
            }
            return View(objList);
        }

        //GET - UpdateAndInsert
        public IActionResult UpdateAndInsert(int? id)
        {
            ProductVM productVM = new()
            {
                Product = new Product(),
                CategorySelectList = db.Categories.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.CategoryId.ToString()
                }),
               /* ApplicationTypeSelectList = db.ApplicationTypes.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.ApplicationTypeId.ToString()
                })*/
            };
            if (id == null)
            {
                return View(productVM);
            }
            else
            {
                productVM.Product = db.Products.Find(id);
                if (productVM.Product == null) return NotFound();
                return View(productVM);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateAndInsert(ProductVM? productVM)
        {
            if (productVM.Product.CategoryId != 0 && ModelState["Product.CategoryID"]!.ValidationState == ModelValidationState.Valid)
                ModelState["Product.Category"]!.ValidationState = ModelValidationState.Valid;

            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                string webRootPath = webHostEnvironment.WebRootPath;
                if (productVM.Product.ProductId == 0)
                {
                    string upload = webRootPath + WebConstants.ImagePath;
                    string fileName = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(files[0].FileName);

                    using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }

                    productVM.Product.Image = fileName + extension;
                    db.Products.Add(productVM.Product);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    var product = db.Products.AsNoTracking().FirstOrDefault(u => u.ProductId == productVM.Product.ProductId);
                    if (files.Count > 0)
                    {
                        string upload = webRootPath + WebConstants.ImagePath;
                        string fileName = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(files[0].FileName);


                        var oldFile = Path.Combine(upload, product.Image);

                        if (System.IO.File.Exists(oldFile))
                        {
                            System.IO.File.Delete(oldFile);
                        }

                        using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        }

                        productVM.Product.Image = fileName + extension;
                    }
                    else
                    {
                        productVM.Product.Image = product.Image;
                    }
                    db.Products.Update(productVM.Product);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            else
            {
                foreach (var modelState in ViewData.ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        Console.WriteLine(error.ErrorMessage);
                    }
                }

            }
            productVM.CategorySelectList = db.Categories.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.CategoryId.ToString()
            });
            /*productVM.ApplicationTypeSelectList = db.ApplicationTypes.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.ApplicationTypeId.ToString()
            });*/
            return View(productVM);
        }


        //GET - Delete
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var product = db.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }
            DeletePost(id);
            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? productId)
        {
            var product = db.Products.Find(productId);
            if (product == null)
            {
                return NotFound();
            }
            System.IO.File.Delete($"{Directory.GetCurrentDirectory()}/wwwroot/images/product/{product.Image}");
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
