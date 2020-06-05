using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EduMark.Models;

namespace EduMark.Controllers
{
    public class RegisterController : Controller
    {
        private readonly ApplicationDbContext _db;
        [BindProperty]
        public User UserPj { get; set; }
        public RegisterController(ApplicationDbContext db)
        {
            _db = db;
        }
        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
        }

        public IActionResult Index(int? id)
        {
            UserPj = new User();
            if (id == null)
            {
                //create
                return View(UserPj);
            }
            //update
            UserPj = _db.Users.FirstOrDefault(u => u.Id == id);
            if (UserPj == null)
            {
                return NotFound();
            }
            return View(UserPj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index()
        {
            if (ModelState.IsValid)
            {
                if (UserPj.Id == 0)
                {
                    //create
                    _db.Users.Add(UserPj);
                }
                else
                {
                    _db.Users.Update(UserPj);
                }
                _db.SaveChanges();
                return View("Reg_Success", UserPj);
                //return RedirectToAction("/Index");
            }
            return View(User);
        }
    }
}
