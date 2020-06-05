using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using EduMark.Models;


namespace EduMark.Controllers
{
    public class TeacherController : Controller
    {




        private readonly ApplicationDbContext _db;
        public TeacherController(ApplicationDbContext db)
        {
            _db = db;
        }
        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
        }

        public IActionResult Index()
        {
            var email = HttpContext.Session.GetString("email");
            var role = HttpContext.Session.GetString("role");
            if (email == null || role != "teacher")
            {
                return RedirectToAction("Login");
            }
            int teacherId = (int)HttpContext.Session.GetInt32("userId");
            if (teacherId == 0)
            {
                return Json(new { success = false, message = "Login is expired!" });
            }

            var teacher = _db.Users.FirstOrDefaultAsync(u => u.Id == teacherId);
            return View(teacher.Result);          
        }




        public IActionResult Main()
        {      
            var email = HttpContext.Session.GetString("email");
            var role = HttpContext.Session.GetString("role");
            if (email == null || role != "teacher")
            {
                return RedirectToAction("Login");
            }
            int teacherId = (int)HttpContext.Session.GetInt32("userId");
            if (teacherId == 0)
            {
                return Json(new { success = false, message = "Login session is expired!" });
            }
            var today = DateTime.Today;

            string[] Days = new string[7];
            string[,] Hours = new string[7, 8];
            for (int i = 1; i < 8; i++)
            {
                var day = today.AddDays(i).ToString("yyyy-MM-dd");
                Days[i - 1] = day;
                for (int j = 9; j < 17; j++)
                {
                    string datetime = day + " " + j + ":00:00";
                    DateTime dt1 = DateTime.Parse(datetime);
                    var available = _db.Availabilities.FirstOrDefaultAsync(u => u.TeacherId == teacherId && u.StartTime == dt1);
                    Availability result = available.Result;
                    Hours[i - 1, j - 9] = (result == null) ? "0" : "1";
                }
            }
            ViewBag.Days = Days;
            ViewBag.Hours = Hours;

            List<Appointment> appts = _db.Appointments.Include(p => p.Student).Include(a => a.Availability).ThenInclude(t => t.Teacher).Where(p => p.Availability.TeacherId == teacherId).ToList();
            ViewBag.Appts = appts;

            var teacher = _db.Users.FirstOrDefaultAsync(u => u.Id == teacherId);
            return View(teacher.Result);
        }

        #region API Calls
        [HttpPost]
       [HttpPost] public async Task<IActionResult> SetAvailable(string[][] dayhours, string[] date)
        {
            if (dayhours == null || date == null)
            {
                return Json(new { success = false, message = " Invalid data!" });
            }
            int teacherId = (int)HttpContext.Session.GetInt32("userId");
            if (teacherId == 0)
            {
                return Json(new { success = false, message = "Login session is expired!" });
            }
            for (int i = 0; i < 7; i++)
            {
                string StartDate = date[i];
                for (int j = 9; j < 17; j++)
                {
                    string datetime = StartDate + " " + j + ":00:00";
                    DateTime dt1 = DateTime.Parse(datetime);
                    DateTime dt2 = dt1.AddHours(1);
                    var available = await _db.Availabilities.FirstOrDefaultAsync(u => u.TeacherId == teacherId && u.StartTime == dt1);
                    Availability result = available;
                    if (result == null && dayhours[i][j - 9] == "1")//insert
                    {
                        Availability availability = new Availability();
                        availability.TeacherId = teacherId;
                        availability.StartTime = dt1;
                        availability.EndTime = dt2;
                        _db.Availabilities.Add(availability);
                        _db.SaveChanges();
                    }
                    if (result != null && dayhours[i][j - 9] == "0")//delete
                    {
                        _db.Availabilities.Remove(result);
                        _db.SaveChanges();
                    }
                }
            }

            return Json(new { success = true, message = "Successfully set the availability!" });
        }



        [HttpPost]
        public async Task<IActionResult> UpdateInfo(int teachId, string fullName,string teachLanguages)
        {
            var updateForm = await _db.Users.FirstOrDefaultAsync(u => u.Id == teachId);
            Console.WriteLine("2");
            if (updateForm == null)
            {
                return Json(new { success = false, message = " Invalid Teacher id!" });
            }
            //User  teacher= new User();

            updateForm.FullName = fullName;
            updateForm.TeachLanguages = teachLanguages;
            _db.Users.Update(updateForm);
            _db.SaveChanges();
            return Json(new { success = true, message = " Update Successfully" });
        }
        #endregion
    }
}