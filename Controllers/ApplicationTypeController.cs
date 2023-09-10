using Microsoft.AspNetCore.Mvc;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class ApplicationTypeController : Controller
    {
        private readonly ApplicationDbContext db;
        public ApplicationTypeController(ApplicationDbContext _db)
        {
            db = _db;
        }

        public IActionResult Index()
        {
            IEnumerable<ApplicationType> applicationTypeList = db.ApplicationTypes;
            return View(applicationTypeList);
        }
        //GET - CREATE
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ApplicationType applicationType)
        {
            db.ApplicationTypes.Add(applicationType);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
