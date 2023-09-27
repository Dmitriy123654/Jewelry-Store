using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApp.Data;
using WebApp.Models;
using WebApp.Models.ViewModels;
using WebApp.Utility;

namespace WebApp.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext db;
        [BindProperty]
        public ProductUserVM productUserVM { get; set; }
        public CartController(ApplicationDbContext _db)
        {
            db = _db;
        }
        public IActionResult Index()
        {
            List<ShoppingCart> shoppingCartList = new();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstants.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstants.SessionCart)!.Any())
            {
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WebConstants.SessionCart)!;
            }
            List<int> productInCart = shoppingCartList.Select(x => x.ProductId).ToList();
            IEnumerable<Product> productList = db.Products.Where(x => productInCart.Contains(x.ProductId));
            return View(productList);
        }

        public IActionResult Remove(int id)
        {
            List<ShoppingCart> shoppingCartList = new();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstants.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstants.SessionCart)!.Any())
            {
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WebConstants.SessionCart)!;
            }
            shoppingCartList.Remove(shoppingCartList.FirstOrDefault(u => u.ProductId == id)!);
            HttpContext.Session.Set(WebConstants.SessionCart, shoppingCartList);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Index")]
        public IActionResult IndexPost()
        {
            return RedirectToAction(nameof(Summary));
        }

        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity!;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            //var userId = User.FindFirstValue(ClaimTypes.Name);
            List<ShoppingCart> shoppingCartList = new();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstants.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstants.SessionCart)!.Any())
            {
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WebConstants.SessionCart)!;
            }
            List<int> productInCart = shoppingCartList.Select(x => x.ProductId).ToList();
            IEnumerable<Product> productList = db.Products.Where(x => productInCart.Contains(x.ProductId));

            productUserVM = new ProductUserVM()
            {
                ApplicationUser = db.ApplicationUsers.FirstOrDefault(u => u.Id == claim!.Value),
                Products = productList
            };
            return View(productUserVM);
        }
    }

}