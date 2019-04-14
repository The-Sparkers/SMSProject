using SMSProject.Models;
using SMSProject.Models.ModelEnums;
using SMSProject.ServiceModules;
using SMSProject.ViewModels.AdminViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMSProject.Controllers
{
    public class NotificationsController : Controller
    {
        /// <summary>
        /// Action to send fee notificatiions to the parents
        /// </summary>
        /// <param name="pId">List of Parent Id's to which the notification ha to be sent</param>
        /// <returns>Redirects to the ViewUnPaidParent action</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SendFeeNotification(IEnumerable<int> pId)
        {
            HttpCookie schoolName = Request.Cookies.Get("schlNm"); //getting the school name from the cookies stored at the time of loging
            if (pId == null || pId.Count() == 0)
            {
                //if there is no parent Id into the list then redirect back to the previous action
                return RedirectToAction("ViewUnPaidParent", new { err = true });
            }
            try
            {
                Parent p;
                string message;
                Notification n;
                foreach (var item in pId)
                {
                    HttpCookie conString = Request.Cookies.Get("rwxgqlb"); //getting the connection string the cookies
                    p = new Parent(item, Cryptography.Decrypt(conString.Value));
                    message = "Your have Rs. " + decimal.Round(p.GetMonthFee(DateTime.Now)) + " un-paid at " + schoolName.Value + ". Please submit all your dues as soon as possible."; //formatting the text to be sent
                    n = new Notification(message, DateTime.Now, NotificationStatuses.ForParent, NotificationTypes.SMS, Cryptography.Decrypt(conString.Value));
                    p.SendNotification(n);
                }
                return RedirectToAction("ViewUnPaidParent", "Parents", new { succ = true }); //redirect back to the action with a success message
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        /// <summary>
        /// Action to Send Notifications to the parents and/or Teachers
        /// </summary>
        /// <param name="s">success flag</param>
        /// <param name="err">error flag</param>
        /// <returns></returns>
        public ActionResult SendNotification(bool s = false, bool err = false)
        {
            try
            {
                ViewBag.Success = s;
                ViewBag.Error = err;
                return View();
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        /// <summary>
        /// Post Method to Send Notification
        /// </summary>
        /// <param name="model">form data</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SendNotification(SendNotificationViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View();
                HttpCookie conString = Request.Cookies.Get("rwxgqlb");
                Notification n = null;
                try
                {
                    n = new Notification(model.Body, DateTime.Now, model.Status, model.Type, Cryptography.Decrypt(conString.Value));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                    return View();
                }
                if (model.Status == NotificationStatuses.ForParent)
                {
                    List<int> lstParentIds = model.Parents.ToList();
                    Parent p;
                    foreach (var item in lstParentIds)
                    {
                        p = new Parent(item, Cryptography.Decrypt(conString.Value));
                        p.SendNotification(n);
                    }
                }
                else if (model.Status == NotificationStatuses.ForTeacher)
                {
                    List<int> lstTeacherIds = model.Teachers.ToList();
                    Teacher t;
                    foreach (var item in lstTeacherIds)
                    {
                        t = new Teacher(item, Cryptography.Decrypt(conString.Value));
                    }
                }
                else if (model.Status == NotificationStatuses.ForAll)
                {
                    List<int> lstParentIds = model.Parents.ToList();
                    Parent p;
                    foreach (var item in lstParentIds)
                    {
                        p = new Parent(item, Cryptography.Decrypt(conString.Value));
                        p.SendNotification(n);
                    }
                    List<int> lstTeacherIds = model.Teachers.ToList();
                    Teacher t;
                    foreach (var item in lstTeacherIds)
                    {
                        t = new Teacher(item, Cryptography.Decrypt(conString.Value));
                    }
                }
                return RedirectToAction("SendNotification", new { s = true });
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        /// <summary>
        /// Post Method to get list of parents to send the notifications for AJAX request
        /// </summary>
        /// <returns>HTML content in string format. Parent Name, CNIC and checkbox</returns>
        [HttpPost]
        public ActionResult GetNotificationParents()
        {
            try
            {
                HttpCookie conString = Request.Cookies.Get("rwxgqlb");
                List<string> data = new List<string>();
                foreach (var item in Parent.GetAllParents(Cryptography.Decrypt(conString.Value)))
                {
                    string html = "<li><div class='notification_desc'><input type='checkbox' value='" + item.ParentId + "' class='chkParent' name='Parents' />" + item.FatherName + "</h6><p>CNIC: " + item.FatherCNIC + "</p></ div><div class='clearfix'></div></li>";
                    data.Add(html);
                }
                return Content(string.Join("", data));
            }
            catch (Exception)
            {
                return Content("");
            }
        }
        /// <summary>
        /// Post Method to get list of teachers to send the notifications for AJAX request
        /// </summary>
        /// <returns>HTML content in string format. Teacher Name, CNIC and checkbox</returns>
        [HttpPost]
        public ActionResult GetNotificationTeachers()
        {
            try
            {
                HttpCookie conString = Request.Cookies.Get("rwxgqlb");
                List<string> data = new List<string>();
                foreach (var item in Teacher.GetAllTeachers(Cryptography.Decrypt(conString.Value)))
                {
                    string html = "<li><div class='notification_desc'><input type='checkbox' value='" + item.StaffId + "' class='chkTeacher' name='Teachers' />" + item.Name + "</h6><p>CNIC: " + item.CNIC + "</p></ div><div class='clearfix'></div></li>";
                    data.Add(html);
                }
                return Content(string.Join("", data));
            }
            catch (Exception)
            {
                return Content("");
            }
        }
    }
}