using SMSProject.ServiceModules;
using SMSProject.ViewModels.AdminViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMSProject.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            HttpCookie conString = new HttpCookie("rwxgqlb");
            conString.Expires = DateTime.Now.AddDays(1);
            conString.Value = Cryptography.Encrypt(ConfigurationManager.ConnectionStrings["ModelConString"].ConnectionString);
            Response.Cookies.Add(conString);
            return RedirectToAction("Dashboard");
        }
        public ActionResult Dashboard()
        {
            HttpCookie conString = Request.Cookies.Get("rwxgqlb");
            DashboardViewModel model = new DashboardViewModel(Cryptography.Decrypt(conString.Value));
            return View(model);
        }
    }
}