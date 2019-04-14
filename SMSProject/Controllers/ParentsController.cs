using PagedList;
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
    public class ParentsController : Controller
    {
        public ActionResult AddParent() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddParent(AddParentViewModel model)
        {
            ///Post Method to Add Parent called on submitting the form
            ///model contains the data entered by the user
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }
                HttpCookie conString = Request.Cookies.Get("rwxgqlb"); //getting connection string from the cookie
                Parent p;
                try
                {
                    p = new Parent(model.FatherName, model.MotherName, model.FCNIC, new Models.HelperModels.MobileNumber(model.FCountryCode, model.FCompanyCode, model.FNumber), new Models.HelperModels.MobileNumber(model.MCountryCode, model.MCompanyCode, model.MNumber), model.Address, model.EmergencyContact, Math.Abs(model.ElgibilityThreshold), Cryptography.Decrypt(conString.Value)); //adding new parent
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                    return View();
                }
                //copying data to view model
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
                //getting student belong to the parent
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
                return View("ViewParentDetails", vpvm); //returning newly added parent details
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        public ActionResult EditParent(int pId)
        {
            ///Action to edit parent details
            ///pId is the parent id of which details have to be edited
            try
            {
                HttpCookie conString = Request.Cookies.Get("rwxgqlb"); //getting connection string from the cookie
                Parent p = new Parent(pId, Cryptography.Decrypt(conString.Value)); //new instance of parent with the given parent id
                //copying the data to the view model
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
                ViewBag.ParentId = p.ParentId; //sending parent Id ot the view through ViewBag
                return View(apvm); //return view with view model
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        public ActionResult ViewParentDetails(int pId)
        {
            ///Action to View Details of a parent 
            ///pTd is the parent Id of which details want to be view
            try
            {
                HttpCookie conString = Request.Cookies.Get("rwxgqlb"); //getting connection string from the cookie
                Parent p = new Parent(pId, Cryptography.Decrypt(conString.Value)); //initializing new instance of parent by using parent Id
                //copying data to model
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
                //getting students belong to the parent
                foreach (var item in p.GetAllStudents())
                {
                    vpvm.StudentsList.Add(new ParentStudent
                    {
                        Class = item.Section.Class.Name,
                        Name = item.Name,
                        StudentId = item.StudentId
                    });
                }
                return View(vpvm); //return view by passing model
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        /// <summary>
        /// Post method to Edit Parent. Gathers the data from the form and submit to this action.
        /// </summary>
        /// <param name="model">Data gathered from the form</param>
        /// <param name="id">Unique Iid of the parent</param>
        /// <returns>Newly Added parent details.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditParent(AddParentViewModel model, int id)
        {
            try
            {
                ViewBag.ParentId = id; //Sends id to the View by using View Bag
                HttpCookie conString = Request.Cookies.Get("rwxgqlb"); //get connection string from the cookies
                Parent p = new Parent(id, Cryptography.Decrypt(conString.Value)); //initalizes new instance by using the parent Id
                try
                {
                    //updating the values into the database
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
                //copying data to the view model to show details of newly added parent
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
                ViewBag.Success = true; //setting success flag to true after successful operation
                return View("ViewParentDetails", vpvm); //returning view to show newly added parent details
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        /// <summary>
        /// Shows the list of all parents present into the database.
        /// Also search for the parent
        /// </summary>
        /// <param name="page">nullable variable to set pagination</param>
        /// <param name="searchName">parameter to search parent by name</param>
        /// <param name="searchCNIC">parameter to search parent by CNIC number</param>
        /// <returns>List view of parents</returns>
        public ActionResult ViewParents(int? page, string searchName = "", string searchCNIC = "")
        {
            try
            {
                ViewBag.SearchName = searchName;
                ViewBag.SearchCNIC = searchCNIC;
                HttpCookie conString = Request.Cookies.Get("rwxgqlb"); //getting the connection string from the cookies
                List<ViewParentsViewModel> lstParent = new List<ViewParentsViewModel>();
                if (searchCNIC != "")
                {
                    //if user have searched by the CNIC
                    System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"^[0-9]{5}[-][0-9]{7}[-][0-9]{1}$"); //making a regualr expression for CNIC number format
                    if (!regex.IsMatch(searchCNIC))
                    {
                        ViewBag.Invalid = true; //sets the flag for invalid entry
                    }
                    else
                    {
                        //if the format is true
                        Parent p = new Parent(searchCNIC, Cryptography.Decrypt(conString.Value)); //initializing new instance by using the CNIC number
                        //addng to the model list
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
                    //if the user have searched by parent name
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
                //converting the data into paged list model to accurately handling the pagging
                PagedList<ViewParentsViewModel> model = new PagedList<ViewParentsViewModel>(lstParent, page ?? 1, 20);
                if (lstParent.Count == 0)
                {
                    ViewBag.Error = true;
                }
                return View(model); //returns the view
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
        /// <summary>
        /// Displays the list of parents who haven't paid their dues
        /// </summary>
        /// <param name="page">nullable int to handle pagging</param>
        /// <param name="err">error flag to control some error</param>
        /// <param name="succ">success flag to control some success</param>
        /// <returns>View showing list of parents</returns>
        public ActionResult ViewUnPaidParent(int? page, bool err = false, bool succ = false)
        {
            ViewBag.Error = err; //sending flag to the view via viewbag
            ViewBag.Success = succ; //sending flag to the view via viewbag
            try
            {
                HttpCookie conString = Request.Cookies.Get("rwxgqlb"); //getting the connection string from the cookies
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
    }
}