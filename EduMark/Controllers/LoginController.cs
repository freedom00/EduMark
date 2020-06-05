using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EduMark.Models;
using Microsoft.AspNetCore.Http;

namespace EduMark.Controllers
{
    public class LoginData
    {
        public string email { get; set; }
        public string password { get; set; }

        public string role { get; set; }

    }

    public class LoginController : Controller
    {
        private readonly ApplicationDbContext _db;

        public LoginController(ApplicationDbContext db)
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
            var email = HttpContext.Session.GetString("email");
            if (email != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        #region API Calls
        [HttpPost]
        public async Task<IActionResult> Index(LoginData logindata)
        {
            if (logindata.role == "student" || logindata.role =="teacher")
            {
                var userFromDb = await _db.Users.FirstOrDefaultAsync(u => u.Email == logindata.email && u.Password == logindata.password);
                if (userFromDb == null)
                {
                    return Json(new { success = false, message = " Invalid username or password!" });
                }
                HttpContext.Session.SetString("email", logindata.email);
                HttpContext.Session.SetString("fullname", userFromDb.FullName);
                HttpContext.Session.SetInt32("userId", userFromDb.Id);
                HttpContext.Session.SetString("role", logindata.role);
                return Json(new { success = true });


            }
            else
            {
                return Json(new { success = false, message = " system error" } );
            }

        }
        #endregion

    }
}
