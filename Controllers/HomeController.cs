using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Diagnostics;
using WebApp.Data;
using WebApp.Models;
using WebApp.Models.ViewModels;

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