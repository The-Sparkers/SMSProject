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
using System.Web.Mvc.Html;

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
            catch(Exception ex)
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
            catch(Exception ex)
            {
                return Content(ex.Message);
            }
        }
        public ActionResult AddStudent2(int pId)
        {
            try
            {
                ViewBag.ParentId = pId;
                return View();
            }
            catch(Exception ex)
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
                StudentDetailsViewModel savm = new StudentDetailsViewModel
                {
                    Id = s.StudentId,
                    BForm = s.BFormNumber,
                    Class = s.Section.Class.Name,
                    Name = s.Name,
                    Section = s.Section.Name,
                    AdmissionNumber = s.AdmissionNumber,
                    DOA = s.DateOfAdmission.ToLongDateString(),
                    DOB = s.DateOfBirth.ToLongDateString(),
                    Fee = decimal.Round(s.MonthlyFee).ToString(),
                    FName = s.Parent.FatherName,
                    Gender = s.Gender + "",
                    PrevInst = s.PreviousInstitute,
                    RollNumber = s.RollNumber.ToString()
                };
                return View("StudentDetails", savm);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        public ActionResult StruckOffStudent()
        {
            return View();
        }
    }
}