using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EduMark.Models;

namespace EduMark.Controllers
{
    public class LogoutController : Controller
    {
        private readonly ApplicationDbContext _db;
        public LogoutController(ApplicationDbContext db)
        {
            _db = db;
        }
        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
        }

        [HttpGet]
        public IActionResult Index()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

    }
}