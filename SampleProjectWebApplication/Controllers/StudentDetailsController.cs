using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    public class StudentDetailsController : Controller
    {
        // GET: StudentDetails
        public ActionResult Index()
        {
            ViewBag.Date = DateTime.Now.ToShortDateString();
            return View();
        }

        public ActionResult Contact()
        {
            TempData["text"] = "This message is from Details";
            return View();
        }

        public ActionResult Details()
        {
            var text = "This is from Details";
            
            return RedirectToAction("Contact");
        }
    }
}