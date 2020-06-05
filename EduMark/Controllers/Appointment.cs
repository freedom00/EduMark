//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using PetClinic.Models;

//namespace PetClinic.Controllers
//{
//    public class AppointmentController : Controller
//    {
//        private readonly ApplicationDbContext _db;
//        public AppointmentController(ApplicationDbContext db)
//        {
//            _db = db;
//        }
//        protected override void Dispose(bool disposing)
//        {
//            _db.Dispose();
//        }

//        public IActionResult Index()
//        {
//            var username = HttpContext.Session.GetString("userName");
//            var usertype = HttpContext.Session.GetString("userType");
//            if (username == null || usertype != "Pet")
//            {
//                return View("~/Views/Login/Index.cshtml");
//            }
//            List<Doctor> doctors = _db.Doctors.Include(d => d.Service).Skip(0).Take(3).ToList();

//            List<Service> services = _db.Services.ToList();
//            ViewBag.Services = services;
//            return View("Index", doctors);
//        }

//        [HttpPost]
//        public async Task<IActionResult> Getdoctors(int? serviceid, int pageindex, int pagesize)
//        {
//            System.Threading.Thread.Sleep(1000);
//            List<Doctor> doctors;
//            if (serviceid != null)
//            {
//                doctors = _db.Doctors.Include(d => d.Service).Where(p => p.Service.Id == serviceid).Skip(pageindex * pagesize).Take(pagesize).ToList();
//            }
//            else
//            {
//                doctors = _db.Doctors.Include(d => d.Service).Skip(pageindex * pagesize).Take(pagesize).ToList();
//            }
//            string partialViewHtml = await ControllerExtensions.RenderViewAsync(this, "Doclist", doctors, true);
//            return Json(new { partialViewHtml });
//        }

//        [HttpPost]
//        public async Task<IActionResult> BookAppointment(int doctorid, string date, string time, string comment)
//        {
//            if (!HttpContext.Session.GetInt32("userId").HasValue)
//            {
//                return Json(new { success = false, message = "Login session is expired!" });
//            }

//            if (doctorid == 0)
//            {
//                return Json(new { success = false, message = "Please select a doctor!" });
//            }

//            if (date == null)
//            {
//                return Json(new { success = false, message = "Please select date and time!" });
//            }

//            DateTime dt = DateTime.Parse(date + " " + time + ":00");
//            var timeslot = await _db.Availabilities.FirstOrDefaultAsync(u => u.DoctorId == doctorid && u.StartTime == dt);
//            Appointment appt = new Appointment();
//            appt.Description = comment;
//            appt.AvailabilityId = timeslot.Id;
//            appt.PetId = (int)HttpContext.Session.GetInt32("userId");
//            appt.Status = "Booked";

//            _db.Appointments.Add(appt);
//            _db.SaveChanges();

//            return Json(new { success = true, message = "Successfully!" });
//        }

//        [HttpPost]
//        public async Task<IActionResult> Getavailable(int doctorId)
//        {
//            if (doctorId == 0)
//            {
//                return Json(new { success = false, message = "Login session is expired!" });
//            }
//            var doctor = await _db.Doctors.Include(d => d.Service).FirstOrDefaultAsync(u => u.Id == doctorId);
//            var today = DateTime.Today;
//            //var tomorrow = today.AddDays(1);
//            string[] Days = new string[7];
//            string[,] Hours = new string[7, 8];
//            for (int i = 1; i < 8; i++)
//            {
//                var day = today.AddDays(i).ToString("yyyy-MM-dd");
//                Days[i - 1] = day;
//                for (int j = 9; j < 17; j++)
//                {
//                    string datetime = day + " " + j + ":00:00";
//                    DateTime dt1 = DateTime.Parse(datetime);
//                    var available = await _db.Availabilities.FirstOrDefaultAsync(u => u.DoctorId == doctorId && u.StartTime == dt1);
//                    string status = "1";
//                    Availability result = available;
//                    if (result != null)
//                    {
//                        status = "0";
//                        var appt = await _db.Appointments.FirstOrDefaultAsync(a => a.AvailabilityId == result.Id);
//                        if (appt != null)
//                        {
//                            status = "2";
//                        }
//                    }

//                    Hours[i - 1, j - 9] = status;
//                }
//            }
//            string docdesc = "<h4 class='card-title'> " + doctor.Name + "</h4 ><p>" + doctor.Service.Name + "</p>";
//            string docimg = doctor.PhotoFile;
//            return Json(new { docimg, docdesc, Days, Hours });
//        }
//    }
//}