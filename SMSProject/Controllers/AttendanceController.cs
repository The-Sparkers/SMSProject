using SMSProject.Models;
using SMSProject.ServiceModules;
using SMSProject.ViewModels.AdminViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMSProject.Controllers
{
    public class AttendanceController : Controller
    {
        /// <summary>
        /// Action which returns a view to load srudent attendance according to a certain class and section
        /// </summary>
        /// <param name="err">error flag</param>
        /// <param name="s">success flag</param>
        /// <returns></returns>
        public ActionResult LoadStudentAttendance(bool err = false, bool s = false)
        {
            ViewBag.Error = err;
            ViewBag.Success = s;
            return View();
        }
        /// <summary>
        /// Post Method to Accept form data and returns the view that contains list of students of a section of a class
        /// </summary>
        /// <param name="model">form data</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LoadStudentAttendance(LoadStudentsViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }
                HttpCookie conString = Request.Cookies.Get("rwxgqlb");
                Section section = new Section(model.Section, Cryptography.Decrypt(conString.Value));
                List<ViewStudentAttendanceViewModel> lstStudents = new List<ViewStudentAttendanceViewModel>();
                foreach (var item in section.GetStudents())
                {
                    lstStudents.Add(new ViewStudentAttendanceViewModel
                    {
                        Id = item.StudentId,
                        IsAbsent = item.GetAbsentStatus(DateTime.Now),
                        Name = item.Name,
                        RollNo = item.RollNumber
                    });
                }
                if (lstStudents.Count == 0)
                {
                    ViewBag.Empty = true;
                }
                ViewBag.SectionId = model.Section;
                return View("ViewStudentAttendance", lstStudents);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        /// <summary>
        /// Post Method to add Attendance of the students
        /// </summary>
        /// <param name="id">list of unique student ids</param>
        /// <param name="secId">unique id of the section from which a student belongs</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitStudentAttendance(IEnumerable<int> id, int secId)
        {
            try
            {
                List<int> lstStudentId = new List<int>();
                if (id != null)
                {
                    lstStudentId = id.ToList();
                }
                HttpCookie conString = Request.Cookies.Get("rwxgqlb");
                Section sec = new Section(secId, Cryptography.Decrypt(conString.Value));
                foreach (var item in sec.GetStudents())
                {
                    bool absentFlag = false;
                    foreach (var item2 in lstStudentId)
                    {
                        if (item.StudentId == item2)
                            absentFlag = true;
                    }
                    item.SetAttandance(DateTime.Now, absentFlag);
                }
                return RedirectToAction("LoadStudentAttendance", new { s = true });
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        /// <summary>
        /// Action returns a view which contains list of staff
        /// </summary>
        /// <param name="s">success flag</param>
        /// <param name="err">error flag</param>
        /// <returns></returns>
        public ActionResult ViewStaffAttendance(bool s = false, bool err = false)
        {
            try
            {
                ViewBag.Success = s;
                ViewBag.Error = err;
                List<ViewStaffAttendanceViewModel> lstStaff = new List<ViewStaffAttendanceViewModel>();
                HttpCookie conString = Request.Cookies.Get("rwxgqlb");
                foreach (var item in Staff.GetAllStaff(Cryptography.Decrypt(conString.Value)))
                {
                    lstStaff.Add(new ViewStaffAttendanceViewModel
                    {
                        CNIC = item.CNIC,
                        Id = item.StaffId,
                        IsAbsent = item.GetAbsentStatus(DateTime.Now),
                        Name = item.Name
                    });
                }
                if (lstStaff.Count == 0)
                {
                    ViewBag.Empty = true;
                }
                return View(lstStaff);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        /// <summary>
        /// Post Method that saves the attendance status of selected staff members
        /// </summary>
        /// <param name="id">List of unique ids of students</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitStaffAttendance(IEnumerable<int> id)
        {
            try
            {
                List<int> lstStaffId = new List<int>();
                if (id != null)
                {
                    lstStaffId = id.ToList();
                }
                HttpCookie conString = Request.Cookies.Get("rwxgqlb");
                foreach (var item in Staff.GetAllStaff(Cryptography.Decrypt(conString.Value)))
                {
                    bool absentFlag = false;
                    foreach (var item2 in lstStaffId)
                    {
                        if (item.StaffId == item2)
                            absentFlag = true;
                    }
                    item.SetAttendance(DateTime.Now, absentFlag);
                }
                return RedirectToAction("ViewStaffAttendance", new { s = true });
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        /// <summary>
        /// Action returns view to load attendance details for a particular student for a certain date
        /// </summary>
        /// <returns></returns>
        public ActionResult LoadStudentAttendanceFor() => View();
        /// <summary>
        /// Post Method which takes data from form and load roll number for the student
        /// </summary>
        /// <param name="model">form data</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LoadStudentAttendanceFor(LoadStudentAttendanceForViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }
                HttpCookie conString = Request.Cookies.Get("rwxgqlb");
                Class c = new Class(model.Class, Cryptography.Decrypt(conString.Value));
                Student s = null;
                foreach (var item in c.GetSections())
                {
                    foreach (var item2 in item.GetStudents())
                    {
                        if (item2.RollNumber == model.RollNo)
                        {
                            s = item2;
                            break;
                        }
                    }
                }
                if (s == null)
                {
                    ViewBag.Empty = true;
                    return View();
                }
                return RedirectToAction("StudentAttendanceFor", new { id = s.StudentId, month = model.Month });
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        /// <summary>
        /// Action to view the details of an attendance for a student
        /// </summary>
        /// <param name="id">unique id of student</param>
        /// <param name="month">month for which the attendance has been loaded</param>
        /// <param name="s">success flag</param>
        /// <param name="err">error flag</param>
        /// <returns></returns>
        public ActionResult StudentAttendanceFor(int id, DateTime month, bool s = false, bool err = false)
        {
            try
            {
                ViewBag.StudentId = id;
                ViewBag.Month = month;
                ViewBag.Success = s;
                ViewBag.Error = err;
                HttpCookie conString = Request.Cookies.Get("rwxgqlb");
                Student student = new Student(id, Cryptography.Decrypt(conString.Value));
                ViewStudentAttendanceForViewModel vsavm = new ViewStudentAttendanceForViewModel
                {
                    Name = student.Name,
                    RollNo = student.RollNumber,
                    Attendance = decimal.Divide((DateTime.DaysInMonth(month.Year, month.Month) - student.GetAbsents(month)), DateTime.DaysInMonth(month.Year, month.Month)) * 100,
                    MonthlyAttendances = new List<MonthlyAttendanceViewModel>()
                };
                foreach (var item in student.GetMonthAttendances(month))
                {
                    vsavm.MonthlyAttendances.Add(new MonthlyAttendanceViewModel
                    {
                        IsAbsent = item.IsAbsent,
                        Date = item.Date.ToLongDateString(),
                        Id = item.AttendanceId
                    });
                }
                return View(vsavm);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        /// <summary>
        /// Action to change the status of a particular attendance
        /// </summary>
        /// <param name="id">unique id for attendance</param>
        /// <param name="isAbsent">absent flag</param>
        /// <returns></returns>
        public ActionResult ChangeAttendanceStatus(long id, bool isAbsent)
        {
            try
            {
                HttpCookie conString = Request.Cookies.Get("rwxgqlb");
                Attendance a = new Attendance(id, Cryptography.Decrypt(conString.Value));
                a.IsAbsent = isAbsent;
                return Redirect(Request.UrlReferrer.AbsoluteUri + "&s=true");
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        /// <summary>
        /// Action to return view to load staff attendance for a particular month
        /// </summary>
        /// <returns></returns>
        public ActionResult LoadStaffAttendanceFor() => View();
        /// <summary>
        /// Post Method to Load Staff Attendace for the entered month by the user
        /// </summary>
        /// <param name="model">form data</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LoadStaffAttendanceFor(LoadStaffAttendanceForViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }
                HttpCookie conString = Request.Cookies.Get("rwxgqlb");
                Staff s = new Staff(model.Staff, Cryptography.Decrypt(conString.Value));
                if (s.Name == null || s.Name == "")
                {
                    ViewBag.Empty = true;
                    return View();
                }
                return RedirectToAction("StaffAttendanceFor", new { id = s.StaffId, month = model.Month });
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        /// <summary>
        /// Action to show the Staff Attendance details for a particular month
        /// </summary>
        /// <param name="id">unique id of student</param>
        /// <param name="month">month</param>
        /// <param name="s">success flag</param>
        /// <param name="err">error flag</param>
        /// <returns></returns>
        public ActionResult StaffAttendanceFor(int id, DateTime month, bool s = false, bool err = false)
        {
            try
            {
                ViewBag.StudentId = id;
                ViewBag.Month = month;
                ViewBag.Success = s;
                ViewBag.Error = err;
                HttpCookie conString = Request.Cookies.Get("rwxgqlb");
                Staff staff = new Staff(id, Cryptography.Decrypt(conString.Value));
                ViewStaffAttendanceForViewModel vsavm = new ViewStaffAttendanceForViewModel
                {
                    Name = staff.Name,
                    CNIC = staff.CNIC,
                    Attendance = decimal.Divide((DateTime.DaysInMonth(month.Year, month.Month) - staff.GetAbsents(month)), DateTime.DaysInMonth(month.Year, month.Month)) * 100,
                    MonthlyAttendances = new List<MonthlyAttendanceViewModel>()
                };
                foreach (var item in staff.GetMonthAttendances(month))
                {
                    vsavm.MonthlyAttendances.Add(new MonthlyAttendanceViewModel
                    {
                        IsAbsent = item.IsAbsent,
                        Date = item.Date.ToLongDateString(),
                        Id = item.AttendanceId
                    });
                }
                return View(vsavm);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

    }
}