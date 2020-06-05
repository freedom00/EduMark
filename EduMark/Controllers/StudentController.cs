using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduMark.Models;
using Microsoft.AspNetCore.Mvc;

namespace EduMark.Controllers
{
    public class StudentController : Controller
    {

        private readonly ApplicationDbContext _db;
        public StudentController(ApplicationDbContext db)
        {
            _db = db;
        }
        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}