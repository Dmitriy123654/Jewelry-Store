using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Diagnostics;
using WebApp.Data;
using WebApp.Models;
using WebApp.Models.ViewModels;
using WebApp.Utility;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext db;
        private static HomeVM? homeVMAllProducts;
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext _db)
        {
            _logger = logger;
            db = _db;
            homeVMAllProducts = new HomeVM()
            {
                Products = db.Products.Include(u => u.Category)
                                      .Include(u => u.ApplicationType),
                Categories = db.Categories

            };
        }
        //Get
        public IActionResult Index()
        {
            
            return View(homeVMAllProducts);
        }
        [HttpPost]
        public IActionResult IndexPost(string? category, string JsonHomeVM="")
        {
            var homeVM = JsonConvert.DeserializeObject<HomeVM>(JsonHomeVM);
            if (category == "all")
            {
                return View("Index", homeVMAllProducts);
            }
            else
            {
                var viewModel = new HomeVM
                {
                    Products = db.Products.Where(p => p.Category.Name == category)
                                                                .Include(p => p.Category)
                                                                .Include(p => p.ApplicationType),
                    Categories = homeVM.Categories
            };
                return View("Index", viewModel);
            }

        }

        public IActionResult Details(int id)
        {
            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstants.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstants.SessionCart)!.Count() > 0)
            {
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WebConstants.SessionCart)!;
            }
            DetailsVM detailsVM = new DetailsVM()
            {
                Product = db.Products.Include(u => u.Category)
                                     .Include(u => u.ApplicationType)
                                     .Where(u => u.ProductId == id).FirstOrDefault(),
                ExistsInCart = false
            };
            foreach (var item in shoppingCartList)
            {
                if(item.ProductId == id)
                {
                    detailsVM.ExistsInCart = true;
                }
            }
            
            return View(detailsVM);
        }
        [HttpPost, ActionName("Details")]
        public IActionResult DetailsPost(int id)
        {
            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
            if(HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstants.SessionCart)!= null 
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstants.SessionCart)!.Count() > 0)
            {
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WebConstants.SessionCart)!;
            }
            shoppingCartList.Add( new ShoppingCart { ProductId = id });
            HttpContext.Session.Set(WebConstants.SessionCart, shoppingCartList);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult RemoveFromCart(int id)
        {
            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstants.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstants.SessionCart)!.Count() > 0)
            {
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WebConstants.SessionCart)!;
            }
            var itemToRemove = shoppingCartList.SingleOrDefault(u => u.ProductId == id);
            if(itemToRemove != null)
            {
                shoppingCartList.Remove(itemToRemove);
            }
            HttpContext.Session.Set(WebConstants.SessionCart, shoppingCartList);
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}