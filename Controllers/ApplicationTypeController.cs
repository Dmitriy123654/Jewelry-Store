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
            if (ModelState.IsValid)
            {
                db.ApplicationTypes.Add(applicationType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(applicationType);
        }

        //GET - EDIT
        public IActionResult Edit(int? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }
            var applicationType = db.ApplicationTypes.Find(id);
            if (applicationType == null)
            {
                return NotFound();
            }
            return View(applicationType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ApplicationType applicationType)
        {
            if (ModelState.IsValid)
            {
                db.Update(applicationType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
            return View(applicationType);
        }


        //GET - DELETE
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var applicationType = db.ApplicationTypes.Find(id);
            if (applicationType == null)
            {
                return NotFound();
            }
            return View(applicationType);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(ApplicationType applicationType)
        {
           
            db.ApplicationTypes.Remove(applicationType);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
