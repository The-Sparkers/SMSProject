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

namespace SMSProject.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            try
            {
                HttpCookie conString = new HttpCookie("rwxgqlb");
                conString.Expires = DateTime.Now.AddDays(1);
                conString.Value = Cryptography.Encrypt(ConfigurationManager.ConnectionStrings["ModelConString"].ConnectionString);
                Response.Cookies.Add(conString);
                HttpCookie schoolName = new HttpCookie("schlNm");
                schoolName.Expires = DateTime.Now.AddDays(1);
                schoolName.Value = "Demo";
                Response.Cookies.Add(schoolName);
                return RedirectToAction("Dashboard");
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
                HttpCookie conString = Request.Cookies.Get("rwxgqlb");
                DashboardViewModel model = new DashboardViewModel(Cryptography.Decrypt(conString.Value));
                return View(model);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        public ActionResult AddStudent1()
        {
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult AddStudent1(AddStudent1ViewModel model, bool err = false)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            try
            {
                HttpCookie conString = Request.Cookies.Get("rwxgqlb");
                if (model.SearchCNIC != null)
                {
                    model.SearchResult = new List<AddStudent1SearchResultViewModel>();
                    foreach (var item in Parent.GetAllParentsByCNIC(Cryptography.Decrypt(conString.Value), model.SearchCNIC))
                    {
                        model.SearchResult.Add(new AddStudent1SearchResultViewModel
                        {
                            FatherCNIC = item.FatherCNIC,
                            FatherName = item.FatherName,
                            MotherName = item.MotherName,
                            ParentId = item.ParentId
                        });
                    }
                }
                else if (model.SearchName != null)
                {
                    model.SearchResult = new List<AddStudent1SearchResultViewModel>();
                    foreach (var item in Parent.GetAllParents(Cryptography.Decrypt(conString.Value), model.SearchName))
                    {
                        model.SearchResult.Add(new AddStudent1SearchResultViewModel
                        {
                            FatherCNIC = item.FatherCNIC,
                            FatherName = item.FatherName,
                            MotherName = item.MotherName,
                            ParentId = item.ParentId
                        });
                    }
                }
                else
                {
                    return RedirectToAction("AddStudent1");
                }
                if (model.SearchResult.Count == 0)
                {
                    err = true;
                }
                ViewBag.error = err;
                return View(model);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        public ActionResult AddStudent2(int pId, string returnAction = "")
        {
            TempData.Add("returnAction", returnAction);
            try
            {
                ViewBag.ParentId = pId;
                return View();
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddStudent2(AddStudent2ViewModel model, int pId)
        {
            try
            {
                ViewBag.ParentId = pId;
                if (!ModelState.IsValid)
                {
                    return View();
                }
                model.ParentId = pId;
                TempData["reToAddStudent3"] = model;
                return RedirectToAction("AddStudent3");
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        [HttpGet]
        public ActionResult AddStudent3()
        {
            try
            {
                AddStudent2ViewModel data = (AddStudent2ViewModel)TempData["reToAddStudent3"];
                AddStudent3ViewModel asvm = new AddStudent3ViewModel()
                {
                    AddmissionNumber = data.AddmissionNumber,
                    Name = data.Name,
                    BForm = data.BForm,
                    DOB = data.DOB,
                    Gender = data.Gender,
                    MonthlyFee = data.MonthlyFee,
                    Prevnst = data.Prevnst,
                    ParentId = data.ParentId,
                    Class = data.Class
                };

                return View(asvm);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult AddStudent3(AddStudent3ViewModel model)
        {
            try
            {

                ViewBag.Class = model.Class;
                ViewBag.ParentId = model.ParentId;
                if (!ModelState.IsValid)
                {
                    return View();
                }
                HttpCookie conString = Request.Cookies.Get("rwxgqlb");
                Student s;
                try
                {
                    if (model.BForm == null && model.Prevnst == null)
                    {
                        s = new Student(model.Name, model.AddmissionNumber, model.DOB, DateTime.Now, model.MonthlyFee, model.Gender, model.ParentId, model.Section, Cryptography.Decrypt(conString.Value));
                    }
                    else if (model.BForm == null && model.Prevnst != null)
                    {
                        s = new Student(model.Name, model.AddmissionNumber, model.DOB, DateTime.Now, model.MonthlyFee, model.Gender, model.ParentId, model.Section, Cryptography.Decrypt(conString.Value), "", model.Prevnst);
                    }
                    else if (model.BForm != null && model.Prevnst == null)
                    {
                        s = new Student(model.Name, model.AddmissionNumber, model.DOB, DateTime.Now, model.MonthlyFee, model.Gender, model.ParentId, model.Section, Cryptography.Decrypt(conString.Value), model.BForm, "");
                    }
                    else
                    {
                        s = new Student(model.Name, model.AddmissionNumber, model.DOB, DateTime.Now, model.MonthlyFee, model.Gender, model.ParentId, model.Section, Cryptography.Decrypt(conString.Value), model.BForm, model.Prevnst);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                    return View();

                }
                string returnAction = (string)TempData["returnAction"];
                if (returnAction != "" || (returnAction != null && returnAction != ""))
                {
                    TempData.Remove("returnAction");
                    return RedirectToAction(returnAction, new { pId = s.Parent.ParentId });
                }
                StudentDetailsViewModel savm = new StudentDetailsViewModel
                {
                    Id = s.StudentId,
                    BForm = s.BFormNumber,
                    Class = s.Section.Class.Name,
                    Name = s.Name,
                    Section = s.Section.Name,
                    AdmissionNumber = s.AdmissionNumber,
                    DOA = s.DateOfAdmission.ToLongDateString(),
                    Age = s.Age.ToString(),
                    Fee = decimal.Round(s.MonthlyFee).ToString(),
                    FName = s.Parent.FatherName,
                    Gender = s.Gender + "",
                    PrevInst = s.PreviousInstitute,
                    RollNumber = s.RollNumber.ToString()
                };
                ViewBag.Success = true;
                return View("ViewStudentDetails", savm);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        public ActionResult ViewStudentDetails(int id)
        {
            try
            {
                HttpCookie conString = Request.Cookies.Get("rwxgqlb");
                Student s = new Student(id, Cryptography.Decrypt(conString.Value));
                StudentDetailsViewModel sdvm = new StudentDetailsViewModel
                {
                    AdmissionNumber = s.AdmissionNumber,
                    Age = s.Age.ToString(),
                    BForm = s.BFormNumber,
                    Class = s.Section.Class.Name,
                    Name = s.Name,
                    Section = s.Section.Name,
                    DOA = s.DateOfAdmission.ToLongDateString(),
                    Fee = decimal.Round(s.MonthlyFee).ToString(),
                    FName = s.Parent.FatherName,
                    Gender = s.Gender + "",
                    Id = s.StudentId,
                    PrevInst = s.PreviousInstitute,
                    RollNumber = s.RollNumber.ToString()
                };
                return View(sdvm);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        public ActionResult EditStudent(int id)
        {
            try
            {
                HttpCookie conString = Request.Cookies.Get("rwxgqlb");
                Student s = new Student(id, Cryptography.Decrypt(conString.Value));
                AddStudent2ViewModel asvm = new AddStudent2ViewModel
                {
                    AddmissionNumber = s.AdmissionNumber,
                    BForm = s.BFormNumber,
                    DOB = s.DateOfBirth,
                    Gender = s.Gender,
                    MonthlyFee = s.MonthlyFee,
                    Name = s.Name,
                    Prevnst = s.PreviousInstitute
                };
                ViewBag.StudentId = s.StudentId;
                return View(asvm);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditStudent(AddStudent2ViewModel model, int sId)
        {
            try
            {
                ViewBag.StudentId = sId;
                HttpCookie conString = Request.Cookies.Get("rwxgqlb");
                Student s = new Student(sId, Cryptography.Decrypt(conString.Value));
                try
                {
                    s.AdmissionNumber = model.AddmissionNumber;
                    s.BFormNumber = model.BForm;
                    s.DateOfBirth = model.DOB;
                    s.Gender = model.Gender;
                    s.MonthlyFee = model.MonthlyFee;
                    s.Name = model.Name;
                    s.PreviousInstitute = model.Prevnst;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                    return View();
                }
                StudentDetailsViewModel sdvm = new StudentDetailsViewModel
                {
                    AdmissionNumber = s.AdmissionNumber,
                    Name = s.Name,
                    Age = s.Age.ToString(),
                    BForm = s.BFormNumber,
                    Class = s.Section.Class.Name,
                    Section = s.Section.Name,
                    DOA = s.DateOfAdmission.ToLongDateString(),
                    Fee = decimal.Round(s.MonthlyFee).ToString(),
                    FName = s.Parent.FatherName,
                    Gender = s.Gender + "",
                    Id = s.StudentId,
                    PrevInst = s.PreviousInstitute,
                    RollNumber = s.RollNumber.ToString()
                };
                ViewBag.Success = true;
                return View("ViewStudentDetails", sdvm);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        public ActionResult StruckOffStudent(bool s = false)
        {
            ViewBag.Success = s;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult StruckOffStudent(StruckOffStudentViewModel model, int? page, bool err = false)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            try
            {
                HttpCookie conString = Request.Cookies.Get("rwxgqlb");
                model.Result = new List<StruckOffStudentSearchResult>();
                foreach (var item in Student.Search(model.SearchName, Cryptography.Decrypt(conString.Value)))
                {
                    model.Result.Add(new StruckOffStudentSearchResult
                    {
                        Class = item.Section.Class.Name,
                        FatherName = item.Parent.FatherName,
                        StudentId = item.StudentId,
                        StudentName = item.Name,
                        AdmissionNumber = item.AdmissionNumber
                    });
                }
                if (model.Result.Count == 0)
                {
                    err = true;
                }
                ViewBag.error = err;
                return View(model);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        public ActionResult StruckOffStudentDone(int id, string bRoll)
        {
            try
            {
                HttpCookie conString = Request.Cookies.Get("rwxgqlb");
                Student s = new Student(id, Cryptography.Decrypt(conString.Value));
                StruckOffStudent st = new StruckOffStudent(s, bRoll, Cryptography.Decrypt(conString.Value));
                return RedirectToAction("StruckOffStudent", new { s = true });
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        [HttpGet]
        public ActionResult ViewStruckOffStudents(int? page, string SearchName = "")
        {
            ViewBag.Search = SearchName;
            bool err = false;
            try
            {
                HttpCookie conString = Request.Cookies.Get("rwxgqlb");
                List<ViewStruckOffStudentViewModel> result = new List<ViewStruckOffStudentViewModel>();
                foreach (var item in Models.StruckOffStudent.GetAllStruckedStudents(SearchName, Cryptography.Decrypt(conString.Value)))
                {
                    result.Add(new ViewStruckOffStudentViewModel
                    {
                        BFormNumber = item.BFormNumber,
                        BoardExam = item.BoardExamRollNumber,
                        Contact = item.FatherCellNo,
                        DOS = item.DateOfStruck.ToLongDateString(),
                        FatherName = item.FatherName,
                        Gender = item.Gender + "",
                        LastClass = item.LastClass,
                        StudentName = item.Name
                    });
                }
                PagedList<ViewStruckOffStudentViewModel> model = new PagedList<ViewStruckOffStudentViewModel>(result, page ?? 1, 20);
                if (model.Count == 0)
                    err = true;
                ViewBag.Error = err;
                return View(model);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        public ActionResult _SearchStruckOffPartial()
        {
            return PartialView();
        }
        public ActionResult AddParent()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddParent(AddParentViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }
                HttpCookie conString = Request.Cookies.Get("rwxgqlb");
                Parent p;
                try
                {
                    p = new Parent(model.FatherName, model.MotherName, model.FCNIC, new Models.HelperModels.MobileNumber(model.FCountryCode, model.FCompanyCode, model.FNumber), new Models.HelperModels.MobileNumber(model.MCountryCode, model.MCompanyCode, model.MNumber), model.Address, model.EmergencyContact, Math.Abs(model.ElgibilityThreshold), Cryptography.Decrypt(conString.Value));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                    return View();
                }
                ViewParentDetailsViewModel vpvm = new ViewParentDetailsViewModel
                {
                    Address = p.HomeAddress,
                    ElgibilityThreshold = p.EligibiltyThreshold,
                    EmergencyContact = p.EmergencyContact,
                    FatherName = p.FatherName,
                    FCNIC = p.FatherCNIC,
                    FNumber = p.FatherMobile.GetLocalViewFormat(),
                    MNumber = p.MotherMobile.GetLocalViewFormat(),
                    MotherName = p.MotherName,
                    ParentId = p.ParentId,
                    StudentsList = new List<ParentStudent>()
                };
                foreach (var item in p.GetAllStudents())
                {
                    vpvm.StudentsList.Add(new ParentStudent
                    {
                        Class = item.Section.Class.Name,
                        Name = item.Name,
                        StudentId = item.StudentId
                    });
                }
                ViewBag.Success = true;
                return View("ViewParentDetails", vpvm);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        public ActionResult EditParent(int pId)
        {
            try
            {
                HttpCookie conString = Request.Cookies.Get("rwxgqlb");
                Parent p = new Parent(pId, Cryptography.Decrypt(conString.Value));
                AddParentViewModel apvm = new AddParentViewModel
                {
                    Address = p.HomeAddress,
                    ElgibilityThreshold = p.EligibiltyThreshold,
                    EmergencyContact = p.EmergencyContact,
                    FatherName = p.FatherName,
                    FCNIC = p.FatherCNIC,
                    FCompanyCode = p.FatherMobile.CompanyCode,
                    FCountryCode = p.FatherMobile.CountryCode,
                    FNumber = p.FatherMobile.Number,
                    MCompanyCode = p.MotherMobile.CompanyCode,
                    MCountryCode = p.MotherMobile.CountryCode,
                    MNumber = p.MotherMobile.Number,
                    MotherName = p.MotherName
                };
                ViewBag.ParentId = p.ParentId;
                return View(apvm);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        public ActionResult ViewParentDetails(int pId)
        {
            try
            {
                HttpCookie conString = Request.Cookies.Get("rwxgqlb");
                Parent p = new Parent(pId, Cryptography.Decrypt(conString.Value));
                ViewParentDetailsViewModel vpvm = new ViewParentDetailsViewModel
                {
                    Address = p.HomeAddress,
                    ElgibilityThreshold = p.EligibiltyThreshold,
                    EmergencyContact = p.EmergencyContact,
                    FatherName = p.FatherName,
                    FCNIC = p.FatherCNIC,
                    FNumber = p.FatherMobile.GetLocalViewFormat(),
                    MNumber = p.MotherMobile.GetLocalViewFormat(),
                    MotherName = p.MotherName,
                    ParentId = p.ParentId,
                    StudentsList = new List<ParentStudent>()
                };
                foreach (var item in p.GetAllStudents())
                {
                    vpvm.StudentsList.Add(new ParentStudent
                    {
                        Class = item.Section.Class.Name,
                        Name = item.Name,
                        StudentId = item.StudentId
                    });
                }
                return View(vpvm);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditParent(AddParentViewModel model, int id)
        {
            try
            {
                ViewBag.ParentId = id;
                HttpCookie conString = Request.Cookies.Get("rwxgqlb");
                Parent p = new Parent(id, Cryptography.Decrypt(conString.Value));
                try
                {
                    p.EligibiltyThreshold = model.ElgibilityThreshold;
                    p.EmergencyContact = model.EmergencyContact;
                    p.FatherCNIC = model.FCNIC;
                    p.FatherMobile = new Models.HelperModels.MobileNumber(model.FCountryCode, model.FCompanyCode, model.FNumber);
                    p.FatherName = model.FatherName;
                    p.HomeAddress = model.Address;
                    p.MotherMobile = new Models.HelperModels.MobileNumber(model.MCountryCode, model.MCompanyCode, model.MNumber);
                    p.MotherName = model.MotherName;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                    return View();
                }
                ViewParentDetailsViewModel vpvm = new ViewParentDetailsViewModel
                {
                    Address = p.HomeAddress,
                    ElgibilityThreshold = p.EligibiltyThreshold,
                    EmergencyContact = p.EmergencyContact,
                    FatherName = p.FatherName,
                    FCNIC = p.FatherCNIC,
                    FNumber = p.FatherMobile.GetLocalViewFormat(),
                    MNumber = p.MotherMobile.GetLocalViewFormat(),
                    MotherName = p.MotherName,
                    ParentId = p.ParentId,
                    StudentsList = new List<ParentStudent>()
                };
                foreach (var item in p.GetAllStudents())
                {
                    vpvm.StudentsList.Add(new ParentStudent
                    {
                        Class = item.Section.Class.Name,
                        Name = item.Name,
                        StudentId = item.StudentId
                    });
                }
                ViewBag.Success = true;
                return View("ViewParentDetails", vpvm);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        public ActionResult ViewParents(int? page, string searchName = "", string searchCNIC = "")
        {
            try
            {
                ViewBag.SearchName = searchName;
                ViewBag.SearchCNIC = searchCNIC;
                HttpCookie conString = Request.Cookies.Get("rwxgqlb");
                List<ViewParentsViewModel> lstParent = new List<ViewParentsViewModel>();
                if (searchCNIC != "")
                {
                    System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"^[0-9]{5}[-][0-9]{7}[-][0-9]{1}$");
                    if (!regex.IsMatch(searchCNIC))
                    {
                        ViewBag.Invalid = true;
                    }
                    else
                    {
                        Parent p = new Parent(searchCNIC, Cryptography.Decrypt(conString.Value));
                        lstParent.Add(new ViewParentsViewModel
                        {
                            Balance = decimal.Round(p.Balance).ToString(),
                            FCNIC = p.FatherCNIC,
                            Fname = p.FatherName,
                            MName = p.MotherName,
                            ParentId = p.ParentId
                        });
                    }
                }
                else
                {
                    foreach (var item in Parent.GetAllParents(Cryptography.Decrypt(conString.Value), searchName))
                    {
                        lstParent.Add(new ViewParentsViewModel
                        {
                            Balance = decimal.Round(item.Balance).ToString(),
                            FCNIC = item.FatherCNIC,
                            Fname = item.FatherName,
                            MName = item.MotherName,
                            ParentId = item.ParentId
                        });
                    }
                }
                PagedList<ViewParentsViewModel> model = new PagedList<ViewParentsViewModel>(lstParent, page ?? 1, 20);
                if (lstParent.Count == 0)
                {
                    ViewBag.Error = true;
                }
                return View(model);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        public ActionResult _SearchParentByCNICPartial()
        {
            return PartialView();
        }
        public ActionResult ViewUnPaidParent(int? page, bool err = false, bool succ = false)
        {
            ViewBag.Error = err;
            ViewBag.Success = succ;
            try
            {
                HttpCookie conString = Request.Cookies.Get("rwxgqlb");
                List<ViewUnPaidParentsViewModel> lstParents = new List<ViewUnPaidParentsViewModel>();
                foreach (var item in Parent.GetParentsWithUnpaidDues(Cryptography.Decrypt(conString.Value), DateTime.Now))
                {
                    lstParents.Add(new ViewUnPaidParentsViewModel
                    {
                        Amount = "Rs. " + decimal.Round(item.GetMonthFee(DateTime.Now)).ToString(),
                        FName = item.FatherName,
                        ParentId = item.ParentId
                    });
                }
                if (lstParents.Count == 0)
                {
                    ViewBag.Empty = true;
                }
                PagedList<ViewUnPaidParentsViewModel> model = new PagedList<ViewUnPaidParentsViewModel>(lstParents, page ?? 1, 50);
                return View(model);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SendFeeNotification(IEnumerable<int> pId)
        {
            HttpCookie schoolName = Request.Cookies.Get("schlNm");
            if (pId == null || pId.Count() == 0)
            {
                return RedirectToAction("ViewUnPaidParent", new { err = true });
            }
            try
            {
                Parent p;
                string message;
                Notification n;
                foreach (var item in pId)
                {
                    HttpCookie conString = Request.Cookies.Get("rwxgqlb");
                    p = new Parent(item, Cryptography.Decrypt(conString.Value));
                    message = "Your have Rs. " + decimal.Round(p.GetMonthFee(DateTime.Now)) + " un-paid at " + schoolName.Value + ". Please submit all your dues as soon as possible.";
                    n = new Notification(message, DateTime.Now, NotificationStatuses.ForParent, NotificationTypes.SMS, Cryptography.Decrypt(conString.Value));
                    p.SendNotification(n);
                }
                return RedirectToAction("ViewUnPaidParent", new { succ = true });
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        public ActionResult AddStaff()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddStaff(AddStaffViewModel model)
        {
            try
            {
                HttpCookie conString = Request.Cookies.Get("rwxgqlb");
                if (!ModelState.IsValid)
                {
                    return View();
                }
                if (model.StaffType == StaffTypes.NonTeaching && model.JobType == null)
                {
                    ModelState.AddModelError("JobType", "This field is required");
                    return View();
                }
                NonTeachingStaff nts = null;
                Teacher t = null;
                try
                {
                    if (model.StaffType == StaffTypes.NonTeaching)
                    {
                        nts = new NonTeachingStaff(model.Name, model.CNIC, model.Address, new Models.HelperModels.MobileNumber(model.MCountryCode, model.MCompanyCode, model.MNumber), model.Salary, model.Gender, model.JobType, Cryptography.Decrypt(conString.Value));
                    }
                    else if (model.StaffType == StaffTypes.Teacher)
                    {
                        t = new Teacher(model.Name, model.CNIC, model.Address, new Models.HelperModels.MobileNumber(model.MCountryCode, model.MCompanyCode, model.MNumber), model.Salary, model.Gender, Cryptography.Decrypt(conString.Value));
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                    return View();
                }
                if (model.StaffType == StaffTypes.Teacher && t != null)
                {
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
                    ViewBag.Success = true;
                    return View("ViewTeacherDetails", vtdvm);
                }
                else if (model.StaffType == StaffTypes.NonTeaching && nts != null)
                {
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
                    ViewBag.Success = true;
                    return View("ViewNonStaffDetails", vnvm);
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
        public ActionResult ViewNonStaffDetails(int? id, bool s = false)
        {
            try
            {
                ViewBag.Success = s;
                HttpCookie conString = Request.Cookies.Get("rwxgqlb");
                NonTeachingStaff nts = new NonTeachingStaff(id ?? 2012, Cryptography.Decrypt(conString.Value));
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditStaff(AddStaffViewModel model, int id)
        {
            try
            {
                HttpCookie conString = Request.Cookies.Get("rwxgqlb");
                if (!ModelState.IsValid)
                {
                    ViewBag.StaffId = id;
                    ViewBag.StaffType = model.StaffType;
                    return View();
                }
                if (model.StaffType == StaffTypes.NonTeaching && model.JobType == null)
                {
                    ModelState.AddModelError("JobType", "This field is required");
                    ViewBag.StaffId = id;
                    ViewBag.StaffType = model.StaffType;
                    return View();
                }
                try
                {
                    if (model.StaffType == StaffTypes.NonTeaching)
                    {
                        NonTeachingStaff s = new NonTeachingStaff(id, Cryptography.Decrypt(conString.Value))
                        {
                            Address = model.Address,
                            CNIC = model.CNIC,
                            JobType = model.JobType,
                            Name = model.Name,
                            PhoneNumber = new Models.HelperModels.MobileNumber(model.MCountryCode, model.MCompanyCode, model.MNumber),
                            Salary = model.Salary
                        };
                    }
                    else if (model.StaffType == StaffTypes.Teacher)
                    {
                        Teacher s = new Teacher(id, Cryptography.Decrypt(conString.Value))
                        {
                            Address = model.Address,
                            CNIC = model.CNIC,
                            Name = model.Name,
                            PhoneNumber = new Models.HelperModels.MobileNumber(model.MCountryCode, model.MCompanyCode, model.MNumber),
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddQualification(AddQualificationViewModel model, int id)
        {
            try
            {
                HttpCookie conString = Request.Cookies.Get("rwxgqlb");
                if (!ModelState.IsValid)
                {
                    ViewBag.StaffId = id;
                    return View();
                }
                try
                {
                    Teacher t = new Teacher(id, Cryptography.Decrypt(conString.Value));
                    if (t.AddQualification(new Models.HelperModels.Qualification { Degree = model.Degree, Year = Convert.ToInt16(model.Year) }))
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
        public ActionResult RemoveQualification(int id, int sId)
        {
            try
            {
                HttpCookie conString = Request.Cookies.Get("rwxgqlb");
                Teacher t = new Teacher(sId, Cryptography.Decrypt(conString.Value));
                if (t.RemoveQualification(new Models.HelperModels.Qualification { Id = id }))
                {
                    return RedirectToAction("ViewTeacherDetails", new { id = id, s = true });
                }
                else
                {
                    return RedirectToAction("ViewTeacherDetails", new { id = id, err = true });
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        public ActionResult AddTeacherSection(int id)
        {
            ViewBag.TeacherId = id;
            return View();
        }
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
        [HttpPost]
        public ActionResult GetSections(int id)
        {
            try
            {
                HttpCookie conString = Request.Cookies.Get("rwxgqlb");
                Class c = new Class(id, Cryptography.Decrypt(conString.Value));
                List<string> data = new List<string>();
                data.Add("<option value = '' >Select Section</option>");
                foreach (var item in c.GetSections())
                {
                    data.Add("<option value='" + item.SectionId + "'>" + item.Name + "</option>");
                }
                return Content(string.Join("", data));
            }
            catch (Exception)
            {
                return Content("");
            }
        }
        [HttpPost]
        public ActionResult GetSubjects(int id)
        {
            try
            {
                HttpCookie conString = Request.Cookies.Get("rwxgqlb");
                Class c = new Class(id, Cryptography.Decrypt(conString.Value));
                List<string> data = new List<string>();
                data.Add("<option value = '' >Select Subject</option>");
                foreach (var item in c.GetSubjects())
                {
                    data.Add("<option value='" + item.SubjectId + "'>" + item.Name + "</option>");
                }
                return Content(string.Join("", data));
            }
            catch (Exception)
            {
                return Content("");
            }
        }
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
                PagedList<ViewStaffViewModel> model = new PagedList<ViewStaffViewModel>(lstTeachers, page ?? 1, 20);
                return View(model);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
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
        public ActionResult LoadSetSalaries()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LoadSetSalaries(LoadSetSalariesViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }
                if (model.Month == 0)
                {
                    ModelState.AddModelError("Month", "This field is required.");
                    return View();
                }
                return RedirectToAction("SetSalaries", new { month = (int)model.Month, year = model.Year });
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        public ActionResult SetSalaries(int month, int year, int? page, bool s = false, bool err = false)
        {
            try
            {
                HttpCookie conString = Request.Cookies.Get("rwxgqlb");
                ViewBag.Success = s;
                ViewBag.Error = err;
                ViewBag.Month = new Month { Number = month, Year = year };
                List<SetSalariesViewModel> lstStaff = new List<SetSalariesViewModel>();
                foreach (var item in Staff.GetAllUnsetSalaryStaff(new Month { Number = month, Year = year }, Cryptography.Decrypt(conString.Value)))
                {
                    lstStaff.Add(new SetSalariesViewModel
                    {
                        CNIC = item.CNIC,
                        Id = item.StaffId,
                        Name = item.Name,
                        JoiningDate = item.Joiningdate.ToShortDateString()
                    });
                }
                if (lstStaff.Count == 0)
                {
                    ViewBag.Error = true;
                    return View();
                }
                PagedList<SetSalariesViewModel> model = new PagedList<SetSalariesViewModel>(lstStaff, page ?? 1, 20);
                return View(model);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SetSalary(int id, int month, int year, decimal? perAbsent)
        {
            try
            {
                if (perAbsent == null)
                {
                    return RedirectToAction("SetSalaries", new { month = month, year = year, err = true });
                }
                else
                {
                    HttpCookie conString = Request.Cookies.Get("rwxgqlb");
                    Staff s = new Staff(id, Cryptography.Decrypt(conString.Value));
                    if (s.SetSalary(new Month { Number = month, Year = year }, Math.Abs(perAbsent ?? 0)))
                    {
                        return RedirectToAction("SetSalaries", new { month = month, year = year, s = true });
                    }
                    else
                    {
                        return RedirectToAction("SetSalaries", new { month = month, year = year, err = true });
                    }
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        public ActionResult LoadViewSalaries()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LoadViewSalaries(LoadViewSalariesViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }
                if (model.Month == 0)
                {
                    ModelState.AddModelError("Month", "This field is required.");
                    return View();
                }
                return RedirectToAction("ViewSalaries", new { month = (int)model.Month, year = model.Year });
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        public ActionResult ViewSalaries(int month, int year, int? page)
        {
            try
            {
                HttpCookie conString = Request.Cookies.Get("rwxgqlb");
                ViewBag.Month = new Month { Number = month, Year = year };
                List<ViewSalariesViewModel> lstStaff = new List<ViewSalariesViewModel>();
                Month salaryMonth = new Month { Number = month, Year = year };
                foreach (var item in Staff.GetAllSetSalaryStaff(salaryMonth, Cryptography.Decrypt(conString.Value)))
                {
                    MonthlySalary salary = item.GetMonthSalary(salaryMonth);
                    lstStaff.Add(new ViewSalariesViewModel
                    {
                        CNIC = item.CNIC,
                        Id = item.StaffId,
                        Name = item.Name,
                        Absents = salary.Absents,
                        PerAbsent = salary.PerAbsentDeduction,
                        Salary = salary.Salary,
                        TSalary = salary.TotalSalary
                    });
                }
                if (lstStaff.Count == 0)
                {
                    ViewBag.Error = true;
                }
                PagedList<ViewSalariesViewModel> model = new PagedList<ViewSalariesViewModel>(lstStaff, page ?? 1, 30);
                //Code to make chart
                int cyear = DateTime.Now.Year;
                List<_MonthlySalariesChartPartialViewModel> cmodel = new List<_MonthlySalariesChartPartialViewModel>();
                for (int i = 1; i <= 12; i++)
                {
                    decimal monthTotal = 0;
                    Month cmonth = new Month { Number = i, Year = cyear };
                    foreach (var item in Staff.GetAllSetSalaryStaff(cmonth, Cryptography.Decrypt(conString.Value)))
                    {
                        monthTotal += item.GetMonthSalary(cmonth).TotalSalary;
                    }
                    cmodel.Add(new _MonthlySalariesChartPartialViewModel
                    {
                        Month = cmonth.Name + "",
                        TotalSalaries = decimal.Round(monthTotal).ToString()
                    });
                }
                TempData["MonthlySalaryChart"] = cmodel;
                return View(model);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        public ActionResult _MonthlySalariesChart()
        {
            try
            {
                HttpCookie conString = Request.Cookies.Get("rwxgqlb");
                int year = DateTime.Now.Year;
                List<_MonthlySalariesChartPartialViewModel> model = new List<_MonthlySalariesChartPartialViewModel>();
                for (int i = 1; i <= 12; i++)
                {
                    decimal monthTotal = 0;
                    Month month = new Month { Number = i, Year = year };
                    foreach (var item in Staff.GetAllSetSalaryStaff(month,Cryptography.Decrypt(conString.Value)))
                    {
                        monthTotal += item.GetMonthSalary(month).TotalSalary;
                    }
                    model.Add(new _MonthlySalariesChartPartialViewModel
                    {
                        Month = month.Name + "",
                        TotalSalaries = decimal.Round(monthTotal).ToString()
                    });
                }
                return PartialView(model);
            }
            catch (Exception)
            {
                List<_MonthlySalariesChartPartialViewModel> model = new List<_MonthlySalariesChartPartialViewModel>();
                for (int i = 1; i <= 12; i++)
                {
                    Month month = new Month { Number = i, Year = DateTime.Now.Year };
                    model.Add(new _MonthlySalariesChartPartialViewModel
                    {
                        Month = month.Name + "",
                        TotalSalaries = 0.ToString()
                    });
                }
                return PartialView(model);
            }
        }
    }
}