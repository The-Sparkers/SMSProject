using SMSProject.Models;
using SMSProject.Models.ModelEnums;
using SMSProject.ServiceModules;
using SMSProject.ViewModels.AdminViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using System.Web.Mvc.Html;
using SMSProject.Models.HelperModels;
using System.Threading;

namespace SMSProject.Controllers
{
    public class AdminController : Controller
    {
        public ActionResult Index()
        {
            try
            {
                ///Index action will run when the user successfully logged into the system
                ///It will fetch the connection string and school name from the database and store them into the client's cookies to be used for 1 day
                ///The Index method has no view for itself it just stores the values from the databse to the cookies and redirect it to the dashboard

                HttpCookie conString = new HttpCookie("rwxgqlb"); //set a new cookie to store connection string
                conString.Expires = DateTime.Now.AddDays(1); //set cookie expiry
                //set cookie value to the encypted connection string
                conString.Value = Cryptography.Encrypt(ConfigurationManager.ConnectionStrings["ModelConString"].ConnectionString);
                Response.Cookies.Add(conString); //add the connection string to the client cookies
                HttpCookie schoolName = new HttpCookie("schlNm"); //set a new cookie to store school name of the admin user
                schoolName.Expires = DateTime.Now.AddDays(1); //set cookie expiry
                schoolName.Value = "Demo"; //set value of cookie to the School Name
                Response.Cookies.Add(schoolName); //add cookie to the client cookies
                return RedirectToAction("Dashboard"); //redirect the controller to DASHBOARD ACTION
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        public ActionResult Dashboard()
        {
            try
            {
                ///DASHBOARD ACTION
                HttpCookie conString = Request.Cookies.Get("rwxgqlb"); //retrive the encrypted connection string from the cookie
                DashboardViewModel model = new DashboardViewModel(Cryptography.Decrypt(conString.Value)); //set model for the dashboard 
                return View(model); //return the DASHBOARD VIEW by sending the model
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }




    }
}