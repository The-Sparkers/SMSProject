using PagedList;
using SMSProject.Models;
using SMSProject.Models.HelperModels;
using SMSProject.ServiceModules;
using SMSProject.ViewModels.AdminViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMSProject.Controllers
{
    public class StaffController : Controller
    {
        /// <summary>
        /// Action to return View to add new staff
        /// </summary>
        /// <returns>View containing a form to add</returns>
        public ActionResult AddStaff() => View();
        /// <summary>
        /// Post Action to get submtted data from the add staff form.
        /// </summary>
        /// <param name="model">Contains the data from the add staff form</param>
        /// <returns>View newly added staff details</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddStaff(AddStaffViewModel model)
        {
            try
            {
                HttpCookie conString = Request.Cookies.Get("rwxgqlb"); //getting the connection string from cookies 
                if (!ModelState.IsValid)
                {
                    return View();
                }
                if (model.StaffType == StaffTypes.NonTeaching && model.JobType == null)
                {
                    ModelState.AddModelError("JobType", "This field is required"); //adding model error which displayed on invalid entry
                    return View();
                }
                Staff s = null;
                try
                {
                    if (model.StaffType == StaffTypes.NonTeaching)
                    {
                        s = new NonTeachingStaff(model.Name, model.CNIC, model.Address, new MobileNumber(model.MCountryCode, model.MCompanyCode, model.MNumber), model.Salary, model.Gender, model.JobType, Cryptography.Decrypt(conString.Value));
                    }
                    else if (model.StaffType == StaffTypes.Teacher)
                    {
                        s = new Teacher(model.Name, model.CNIC, model.Address, new MobileNumber(model.MCountryCode, model.MCompanyCode, model.MNumber), model.Salary, model.Gender, Cryptography.Decrypt(conString.Value));
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                    return View();
                }
                if (model.StaffType == StaffTypes.Teacher)
                {
                    return RedirectToAction("ViewTeacherDetails", new { id = s.StaffId });
                }
                else if (model.StaffType == StaffTypes.NonTeaching)
                {
                    return RedirectToAction("ViewNonStaffDetails", new { id = s.StaffId });
                }
                else
                {
                    return View();
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        /// <summary>
        /// Displays the details of a 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="s"></param>
        /// <param name="err"></param>
        /// <returns></returns>
        public ActionResult ViewTeacherDetails(int id, bool s = false, bool err = false)
        {
            ViewBag.Success = s;
            ViewBag.Error = err;
            try
            {
                HttpCookie conString = Request.Cookies.Get("rwxgqlb");
                Teacher t = new Teacher(id, Cryptography.Decrypt(conString.Value));
                ViewTeacherDetailsViewModel vtdvm = new ViewTeacherDetailsViewModel
                {
                    Address = t.Address,
                    CNIC = t.CNIC,
                    Gender = t.Gender + "",
                    Id = t.StaffId,
                    MNumber = t.PhoneNumber.GetLocalViewFormat(),
                    Name = t.Name,
                    Qualifications = new List<TeacherQualification>(),
                    Salary = decimal.Round(t.Salary),
                    Sections = new List<TeacherSection>(),
                    JoiningDate = t.Joiningdate.ToLongDateString()
                };
                foreach (var item in t.Qualifications)
                {
                    vtdvm.Qualifications.Add(new TeacherQualification
                    {
                        Degree = item.Degree,
                        Id = item.Id,
                        Year = item.Year.ToString(),
                        TeacherId = t.StaffId
                    });
                }
                foreach (var item in t.GetAssignedSections())
                {
                    vtdvm.Sections.Add(new TeacherSection
                    {
                        Class = item.Section.Class.Name,
                        Section = item.Section.Name,
                        SectionId = item.Section.SectionId,
                        Subject = item.Subject.Name,
                        SubjectId = item.Subject.SubjectId,
                        TeacherId = t.StaffId
                    });
                }
                return View(vtdvm);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        /// <summary>
        /// View the list of Non-Teaching staff.
        /// </summary>
        /// <param name="id">unique id of the staff</param>
        /// <param name="s">success flag </param>
        /// <returns></returns>
        public ActionResult ViewNonStaffDetails(int id, bool s = false)
        {
            try
            {
                ViewBag.Success = s;
                HttpCookie conString = Request.Cookies.Get("rwxgqlb");
                NonTeachingStaff nts = new NonTeachingStaff(id, Cryptography.Decrypt(conString.Value));
                ViewNonStaffDetailsViewModel vnvm = new ViewNonStaffDetailsViewModel
                {
                    Address = nts.Address,
                    CNIC = nts.CNIC,
                    Gender = nts.Gender + "",
                    Id = nts.StaffId,
                    JobType = nts.JobType,
                    MNumber = nts.PhoneNumber.GetLocalViewFormat(),
                    Name = nts.Name,
                    Salary = decimal.Round(nts.Salary),
                    JoiningDate = nts.Joiningdate.ToLongDateString()
                };
                return View(vnvm);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        /// <summary>
        /// Action to edit staff details
        /// </summary>
        /// <param name="id">Unique id of the staff.</param>
        /// <param name="t">Type of Staff</param>
        /// <returns>View showing staff details</returns>
        public ActionResult EditStaff(int id, StaffTypes t)
        {
            try
            {
                ViewBag.StaffId = id;
                HttpCookie conString = Request.Cookies.Get("rwxgqlb");
                AddStaffViewModel asvm;
                if (t == StaffTypes.NonTeaching)
                {
                    NonTeachingStaff s = new NonTeachingStaff(id, Cryptography.Decrypt(conString.Value));
                    asvm = new AddStaffViewModel
                    {
                        Address = s.Address,
                        CNIC = s.CNIC,
                        Gender = s.Gender,
                        JobType = s.JobType,
                        Salary = s.Salary,
                        MCompanyCode = s.PhoneNumber.CompanyCode,
                        MCountryCode = s.PhoneNumber.CountryCode,
                        MNumber = s.PhoneNumber.Number,
                        Name = s.Name,
                        StaffType = t
                    };
                }
                else if (t == StaffTypes.Teacher)
                {
                    Teacher s = new Teacher(id, Cryptography.Decrypt(conString.Value));
                    asvm = new AddStaffViewModel
                    {
                        Address = s.Address,
                        CNIC = s.CNIC,
                        Gender = s.Gender,
                        Salary = s.Salary,
                        MCompanyCode = s.PhoneNumber.CompanyCode,
                        MCountryCode = s.PhoneNumber.CountryCode,
                        MNumber = s.PhoneNumber.Number,
                        Name = s.Name,
                        StaffType = t
                    };
                }
                else
                {
                    asvm = null;
                }
                return View(asvm);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        /// <summary>
        /// Post action to get form data from the edit staff view
        /// </summary>
        /// <param name="model">Form data</param>
        /// <param name="id">Unique id of the staff</param>
        /// <returns>View containing Staff details</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditStaff(AddStaffViewModel model, int id)
        {
            try
            {
                HttpCookie conString = Request.Cookies.Get("rwxgqlb"); //getting the encrypted connection string from cookies
                if (!ModelState.IsValid)
                {
                    //f the model state is not acorrding to the given format
                    ViewBag.StaffId = id; //sending the staff id back to the view for again processing
                    ViewBag.StaffType = model.StaffType; //sending staff type to the view  via viewbag
                    return View();
                }
                if (model.StaffType == StaffTypes.NonTeaching && model.JobType == null)
                {
                    //if the staff type is non-teaching & job type is not selected 
                    ModelState.AddModelError("JobType", "This field is required"); //adding error meesage 
                    ViewBag.StaffId = id; //sending the staff id back to the view for again processing
                    ViewBag.StaffType = model.StaffType; //sending staff type to the view  via viewbag
                    return View();
                }
                try
                {
                    if (model.StaffType == StaffTypes.NonTeaching)
                    {
                        //if non-teachng staff is selected
                        NonTeachingStaff s = new NonTeachingStaff(id, Cryptography.Decrypt(conString.Value))
                        {
                            Address = model.Address,
                            CNIC = model.CNIC,
                            JobType = model.JobType,
                            Name = model.Name,
                            PhoneNumber = new MobileNumber(model.MCountryCode, model.MCompanyCode, model.MNumber),
                            Salary = model.Salary
                        };
                    }
                    else if (model.StaffType == StaffTypes.Teacher)
                    {
                        //if teaching staff is selected
                        Teacher s = new Teacher(id, Cryptography.Decrypt(conString.Value))
                        {
                            Address = model.Address,
                            CNIC = model.CNIC,
                            Name = model.Name,
                            PhoneNumber = new MobileNumber(model.MCountryCode, model.MCompanyCode, model.MNumber),
                            Salary = model.Salary
                        };
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                    return View();
                }
                if (model.StaffType == StaffTypes.Teacher)
                {
                    return RedirectToAction("ViewTeacherDetails", new { id = id, s = true });
                }
                else
                {
                    return RedirectToAction("ViewNonStaffDetails", new { id = id, s = true });
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        public ActionResult AddQualification(int id)
        {
            try
            {
                ViewBag.StaffId = id;
                return View();
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        /// <summary>
        /// Post method to add qualification of a teacher 
        /// </summary>
        /// <param name="model">values of the qualification</param>
        /// <param name="id">id of the teacher</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddQualification(AddQualificationViewModel model, int id)
        {
            try
            {
                HttpCookie conString = Request.Cookies.Get("rwxgqlb"); //getting the connection string from the cookies
                if (!ModelState.IsValid)
                {
                    ViewBag.StaffId = id;
                    return View();
                }
                try
                {
                    Teacher t = new Teacher(id, Cryptography.Decrypt(conString.Value));
                    if (t.AddQualification(new Qualification { Degree = model.Degree, Year = Convert.ToInt16(model.Year) }))
                    {
                        return RedirectToAction("ViewTeacherDetails", new { id = id, s = true });
                    }
                    else
                    {
                        ViewBag.Error = true;

                        return RedirectToAction("AddQualification", new { id = id });
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                    ViewBag.StaffId = id;
                    return View();
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        /// <summary>
        /// Action to remove qualification
        /// </summary>
        /// <param name="id">qualfication id</param>
        /// <param name="sId">staff id</param>
        /// <returns></returns>
        public ActionResult RemoveQualification(int id, int sId)
        {
            try
            {
                HttpCookie conString = Request.Cookies.Get("rwxgqlb");
                Teacher t = new Teacher(sId, Cryptography.Decrypt(conString.Value));
                if (t.RemoveQualification(new Qualification { Id = id }))
                {
                    return RedirectToAction("ViewTeacherDetails", new { id = sId, s = true });
                }
                else
                {
                    return RedirectToAction("ViewTeacherDetails", new { id = sId, err = true });
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        /// <summary>
        /// Action to assign section to the teacher
        /// </summary>
        /// <param name="id">TeacherId</param>
        /// <returns></returns>
        public ActionResult AddTeacherSection(int id)
        {
            ViewBag.TeacherId = id;
            return View();
        }
        /// <summary>
        /// Post method to add section to the teacher 
        /// </summary>
        /// <param name="model">data from the form</param>
        /// <param name="id">Teacher Id</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddTeacherSection(AddTeacherSectionViewModel model, int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.TeacherId = id;
                }
                HttpCookie conString = Request.Cookies.Get("rwxgqlb");
                try
                {
                    Teacher t = new Teacher(id, Cryptography.Decrypt(conString.Value));
                    if (t.AssignSection(model.Section, model.Subject))
                    {
                        return RedirectToAction("ViewTeacherDetails", new { id = id, s = true });
                    }
                    else
                    {
                        ViewBag.TeacherId = id;
                        ViewBag.Error = true;
                        return View();
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.TeacherId = id;
                    ModelState.AddModelError(string.Empty, ex.Message);
                    return View();
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        /// <summary>
        /// Action to unassign section to the teacher
        /// </summary>
        /// <param name="tId">Teacher Id</param>
        /// <param name="subId">Subject Id</param>
        /// <param name="secId">Section Id</param>
        /// <returns></returns>
        public ActionResult RemoveAssignedSection(int tId, int subId, int secId)
        {
            try
            {
                HttpCookie conString = Request.Cookies.Get("rwxgqlb");
                Teacher t = new Teacher(tId, Cryptography.Decrypt(conString.Value));
                if (t.RemoveSection(secId, subId))
                {
                    return RedirectToAction("ViewTeacherDetails", new { id = tId, s = true });
                }
                else
                {
                    return RedirectToAction("ViewTeacherDetails", new { id = tId, err = true });
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        /// <summary>
        /// Action to view list teachers in the database
        /// </summary>
        /// <param name="page">page number</param>
        /// <param name="searchName">name query to search</param>
        /// <param name="err">error flag</param>
        /// <param name="s">success flag</param>
        /// <returns></returns>
        public ActionResult ViewTeachers(int? page, string searchName = "", bool err = false, bool s = false)
        {
            try
            {
                ViewBag.Error = err;
                ViewBag.Success = s;
                ViewBag.SearchName = searchName;
                HttpCookie conString = Request.Cookies.Get("rwxgqlb");
                List<ViewStaffViewModel> lstTeachers = new List<ViewStaffViewModel>();
                foreach (var item in Teacher.GetAllTeachers(Cryptography.Decrypt(conString.Value), searchName))
                {
                    lstTeachers.Add(new ViewStaffViewModel
                    {
                        CNIC = item.CNIC,
                        Id = item.StaffId,
                        Name = item.Name,
                        PNumber = item.PhoneNumber.GetLocalViewFormat()
                    });
                }
                if (lstTeachers.Count == 0)
                {
                    ViewBag.Error = true;
                }
                PagedList<ViewStaffViewModel> model = new PagedList<ViewStaffViewModel>(lstTeachers, page ?? 1, 20); //paged list for pagination in the view
                return View(model);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        /// <summary>
        /// Action to view non-teaching staff
        /// </summary>
        /// <param name="page">page number</param>
        /// <param name="searchName">quesry to search by name</param>
        /// <param name="err">error flag</param>
        /// <returns></returns>
        public ActionResult ViewNonStaff(int? page, string searchName = "", bool err = false)
        {
            try
            {
                ViewBag.Error = err;
                ViewBag.SearchName = searchName;
                HttpCookie conString = Request.Cookies.Get("rwxgqlb");
                List<ViewStaffViewModel> lstTeachers = new List<ViewStaffViewModel>();
                foreach (var item in NonTeachingStaff.GetAllNonTeachingStaff(Cryptography.Decrypt(conString.Value), searchName))
                {
                    lstTeachers.Add(new ViewStaffViewModel
                    {
                        CNIC = item.CNIC,
                        Id = item.StaffId,
                        Name = item.Name,
                        PNumber = item.PhoneNumber.GetLocalViewFormat()
                    });
                }
                if (lstTeachers.Count == 0)
                {
                    ViewBag.Error = true;
                }
                PagedList<ViewStaffViewModel> model = new PagedList<ViewStaffViewModel>(lstTeachers, page ?? 1, 20);
                return View(model);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        /// <summary>
        /// Action to remove staff
        /// </summary>
        /// <param name="id">staff id</param>
        /// <returns></returns>
        public ActionResult RemoveStaff(int id)
        {
            string previousAction = Request.UrlReferrer.AbsolutePath.ToString();
            try
            {
                HttpCookie conString = Request.Cookies.Get("rwxgqlb");
                Staff s = new Staff(id, Cryptography.Decrypt(conString.Value));
                if (s.Delete())
                {
                    return Redirect(previousAction + "?s=true");
                }
                else
                {
                    return Redirect(previousAction);
                }
            }
            catch (Exception ex)
            {
                System.Data.SqlClient.SqlException sqlEx = (System.Data.SqlClient.SqlException)ex.InnerException;
                if (sqlEx.Number == 547)
                {
                    return Redirect(previousAction);
                }
                return Content(ex.Message);
            }
        }
        /// <summary>
        /// Post method to get Staff for the AJAX request
        /// </summary>
        /// <param name="staffType">Type of the staff</param>
        /// <returns>HTML content in string format. Name & CNIC</returns>
        [HttpPost]
        public ActionResult GetStaff(int staffType)
        {
            try
            {
                HttpCookie conString = Request.Cookies.Get("rwxgqlb");
                List<string> data = new List<string>();
                data.Add("<option value = '' >Select Staff</option>");
                if ((StaffTypes)staffType == StaffTypes.NonTeaching)
                {
                    foreach (var item in NonTeachingStaff.GetAllNonTeachingStaff(Cryptography.Decrypt(conString.Value), ""))
                    {
                        data.Add("<option value='" + item.StaffId + "'>Name: " + item.Name + " CNIC: " + item.CNIC + "</option>");
                    }
                }
                else
                {
                    foreach (var item in Teacher.GetAllTeachers(Cryptography.Decrypt(conString.Value), ""))
                    {
                        data.Add("<option value='" + item.StaffId + "'>Name: " + item.Name + " CNIC: " + item.CNIC + "</option>");
                    }
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