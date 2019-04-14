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
        // GET: Admin
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
        public ActionResult AddStudent1()
        {
            ///ADD_STUDENT ACTION 1
            ///Add student proccess has 3 parts
            ///This controller will return the form to add student 
            ///This controller is used to search the parent by CNIC or Father's name
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult AddStudent1(AddStudent1ViewModel model, bool err = false)
        {
            ///The POST Method to get the values from ADD_STUDENT_1 form
            if (!ModelState.IsValid)
            {
                return View();
            }
            try
            {
                HttpCookie conString = Request.Cookies.Get("rwxgqlb"); //get ecrypted connection string from the cookie
                if (model.SearchCNIC != null)
                {
                    //if the user has searched by the CNIC
                    model.SearchResult = new List<AddStudent1SearchResultViewModel>();
                    //store the search result 
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
                    //if user has searched by the Father's Name
                    model.SearchResult = new List<AddStudent1SearchResultViewModel>();
                    //store the search result
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
                    //if user has searched blank fields
                    return RedirectToAction("AddStudent1");
                }
                if (model.SearchResult.Count == 0)
                {
                    //if no item matches with the search
                    err = true;
                }
                /*
                 * the value of error is used in the view to show that wheather if the search item is found or not
                 */
                ViewBag.error = err; 
                return View(model); //return the view with model containing values
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        public ActionResult AddStudent2(int pId, string returnAction = "")
        {
            ///Part 2 of Add Student
            ///pId is the parent Id to which the student is bieng added
            ///returnAction is the address to the action to which the controller will be redirected
            TempData.Add("returnAction", returnAction); //store the returnAction to the TempData to be used again after encoutering some error to adding new student
            try
            {
                ViewBag.ParentId = pId; //send parent Id to the view by using the viewbag
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
            ///Post Method to get the submitted values from the view
            ///pId is the Parent Id to which the student will be added
            try
            {
                ViewBag.ParentId = pId; //send the parent Id to the view by using view bag
                if (!ModelState.IsValid)
                {
                    return View();
                }
                model.ParentId = pId; //assigning the pId to the Parent Id in the model
                TempData["reToAddStudent3"] = model; //store the model data to the TempData to used it in other action
                return RedirectToAction("AddStudent3"); //redirect the action to the 3rd part of Add student
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        [HttpGet]
        public ActionResult AddStudent3()
        {
            ///Part 3 of the ADD STUDENT ACTION
            ///It is the final step of adding student
            ///The data will be retirve from the temp data stored in ADD_STUDENT_2 action
            try
            {
                AddStudent2ViewModel data = (AddStudent2ViewModel)TempData["reToAddStudent3"]; //get temp data and cast it into the model data
                //assemble the data into new view model
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

                return View(asvm); //return the view model to the view
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
            ///Post method to get submitted data of form in ADD_STUDENT_3 VIEW
            try
            {
                ViewBag.Class = model.Class; //send class id to the view by using view bag
                ViewBag.ParentId = model.ParentId; //send parent id to the view by using view bag
                if (!ModelState.IsValid)
                {
                    return View();
                }
                HttpCookie conString = Request.Cookies.Get("rwxgqlb"); //getting encrypted connection string from the cookie
                Student s; //an object of Student type to enter a new student into database
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
                string returnAction = (string)TempData["returnAction"]; //get path to action to which the controller will be redirected
                if (returnAction != "" || (returnAction != null && returnAction != ""))
                {
                    TempData.Remove("returnAction"); //removes the temp data of return action
                    return RedirectToAction(returnAction, new { pId = s.Parent.ParentId }); //redirect to the action with the parent id
                }
                //showing the details of newly added student
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
                ViewBag.Success = true; //flag to show successful operation
                return View("ViewStudentDetails", savm); //show the view student details view with view model having the student's details
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        public ActionResult ViewStudentDetails(int id)
        {
            ///Action to View Student Details
            ///id is the Student Id which data has to be fetched
            try
            {
                HttpCookie conString = Request.Cookies.Get("rwxgqlb"); //get the encrypted connection string from the cookie
                Student s = new Student(id, Cryptography.Decrypt(conString.Value)); //initialized a new object with the student id
                //coping the values of student to the view model
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
                return View(sdvm); //return the View with the view model containging the student's values
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        public ActionResult EditStudent(int id)
        {
            ///Action to Edit the details of respective student
            ///id is the Student Id of whcih the data has to be edited
            try
            {
                HttpCookie conString = Request.Cookies.Get("rwxgqlb"); //getting the encrypted connection string from cookie
                Student s = new Student(id, Cryptography.Decrypt(conString.Value)); //initialize a new object of Student to get the values
                //copying the student's data into view model
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
                ViewBag.StudentId = s.StudentId; //send the student Id to the view, by using the view bag, in order to use it into edit form
                return View(asvm); //return the view with the view model containing the values
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
            ///Post Method to get the values submitted from the form
            ///model contains the data of student
            ///sId is the Student Id to recognoize the student and update values
            try
            {
                ViewBag.StudentId = sId; //send student Id to the view by using the view bag
                HttpCookie conString = Request.Cookies.Get("rwxgqlb"); //getting the encrypted connection string from the cookie
                Student s = new Student(sId, Cryptography.Decrypt(conString.Value)); //initialzed a new object of Student by the student Id to get the data and update it
                try
                {
                    //setting the properties of Student obeject with the model values
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
                //copying new values to the Student Details View Model
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
                ViewBag.Success = true; //set the flag for the successful operation
                return View("ViewStudentDetails", sdvm); //return STUDENT_DETAILS VIEW with the view model containing the updated values
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        public ActionResult StruckOffStudent(bool s = false)
        {
            ///STRUCK_OFF_STUDENT ACTION will show a student list from which a student will be selected to be strucked-off
            ///s is the flag which will show the successful previous operation
            ///The View will show a search box to search the student, to be strucked-off
            ViewBag.Success = s; 
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult StruckOffStudent(StruckOffStudentViewModel model, int? page, bool err = false)
        {
            ///Post method to get values from the form on submit
            ///page is the null able integer variable which stores the page number to handle pagination for PagedList
            if (!ModelState.IsValid)
            {
                return View();
            }
            try
            {
                HttpCookie conString = Request.Cookies.Get("rwxgqlb"); //getting the encrypted connection string from the cookie
                model.Result = new List<StruckOffStudentSearchResult>(); //initiate the result list
                //adding the values to the result list
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
                    //if there is no result found then set the error flag to true
                    err = true;
                }
                ViewBag.error = err; //send the error flag to the View by using view bag
                return View(model); //
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        public ActionResult StruckOffStudentDone(int id, string bRoll)
        {
            ///Action for strucking off a student
            ///id is the Student Id
            ///bRoll is the board roll number for that student (empty if not occurs)
            try
            {
                HttpCookie conString = Request.Cookies.Get("rwxgqlb"); //getting the encrypted connection string from the cookie
                Student s = new Student(id, Cryptography.Decrypt(conString.Value)); //initialize a new object of Student with the Student Id 
                StruckOffStudent st = new StruckOffStudent(s, bRoll, Cryptography.Decrypt(conString.Value)); //Make that student strucked off
                return RedirectToAction("StruckOffStudent", new { s = true }); //redirect to the main struck-off student action
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        [HttpGet]
        public ActionResult ViewStruckOffStudents(int? page, string searchName = "")
        {
            ///Action to view list of strucked-off students
            ///page is nullable datatype which carries the page number (compulsary for handling the paged list)
            ///searchName carries the name of the student entered in the searchbox
            ViewBag.Search = searchName;
            bool err = false;
            try
            {
                HttpCookie conString = Request.Cookies.Get("rwxgqlb"); //getting connection string from the cookies
                List<ViewStruckOffStudentViewModel> result = new List<ViewStruckOffStudentViewModel>();
                foreach (var item in Models.StruckOffStudent.GetAllStruckedStudents(searchName, Cryptography.Decrypt(conString.Value)))
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
                //convert the student's list into paged list object in order to get pagging on view
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
            ///return partial view for search struck-off students
            return PartialView();
        }
        public ActionResult AddParent()
        {
            ///Action which returns view to add parent
            return View();
        }
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
                return RedirectToAction("ViewUnPaidParent", new { succ = true }); //redirect back to the action with a success message
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
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
                foreach (var item in t .Qualifications)
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
        /// Returns the sections list of a class
        /// Called by the AJAX request
        /// </summary>
        /// <param name="id">Class Id</param>
        /// <returns>HTML Content (in the form of string)</returns>
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
        /// <summary>
        /// Returns the Subjects List of a class
        /// Called by the AJAX request
        /// </summary>
        /// <param name="id">Class Id</param>
        /// <returns>HTML content (in the form of string)</returns>
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
        /// Action to load view for set salaries
        /// </summary>
        /// <returns></returns>
        public ActionResult LoadSetSalaries()
        {
            return View();
        }
        /// <summary>
        /// Post method call on submit the form on Load Salaries View
        /// </summary>
        /// <param name="model">form data</param>
        /// <returns></returns>
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
        /// <summary>
        /// Action to show the salaries to be set accordng to the month
        /// </summary>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <param name="page">Page number</param>
        /// <param name="s">success flag</param>
        /// <param name="err">error flag</param>
        /// <returns></returns>
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
        /// <summary>
        /// Action to set salary of a staff for a certain month
        /// </summary>
        /// <param name="id">Staff Id</param>
        /// <param name="month">Month for the salary is being set</param>
        /// <param name="year">Year from which the month belong to</param>
        /// <param name="perAbsent">Per absent deduction</param>
        /// <returns></returns>
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
        /// <summary>
        /// Action to return the view which will get the month for which the salaries have to be loaded.
        /// </summary>
        /// <returns></returns>
        public ActionResult LoadViewSalaries()
        {
            return View();
        }
        /// <summary>
        /// Post Method to get the form data from LoadViewSalaries
        /// </summary>
        /// <param name="model">form data</param>
        /// <returns></returns>
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
        /// <summary>
        /// Action to view the salaries of a respective month
        /// </summary>
        /// <param name="month">Month of which the salaries is going to load</param>
        /// <param name="year">Year from which the month belong</param>
        /// <param name="page">page number</param>
        /// <returns></returns>
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
        /// <summary>
        /// Action to make chart for monthly salaries
        /// </summary>
        /// <returns>Partial chart view</returns>
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
                    foreach (var item in Staff.GetAllSetSalaryStaff(month, Cryptography.Decrypt(conString.Value)))
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
        /// <summary>
        /// Action to return the add class view
        /// </summary>
        /// <returns></returns>
        public ActionResult AddClass()
        {
            return View();
        }
        /// <summary>
        /// Post Method to get data from form filled in Add class view
        /// </summary>
        /// <param name="model">form data</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddClass(AddClassViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }
                HttpCookie conString = Request.Cookies.Get("rwxgqlb");
                Class c = null;
                try
                {
                    c = new Class(model.Name, model.RollNo, model.TeacherId, Cryptography.Decrypt(conString.Value));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
                ViewClassDetailsViewModel vcdvm = new ViewClassDetailsViewModel();
                if (c != null)
                {
                    vcdvm = new ViewClassDetailsViewModel
                    {
                        Id = c.ClassId,
                        Incharge = c.Incharge.Name,
                        Name = c.Name,
                        RollNo = c.RollNoIndex,
                        Sections = new List<ClassSection>(),
                        Strength = c.Strength,
                        Subjects = new List<ClassSubject>()
                    };
                    foreach (var item in c.GetSections())
                    {
                        vcdvm.Sections.Add(new ClassSection
                        {
                            Id = item.SectionId,
                            Name = item.Name,
                            Strength = item.Strength
                        });
                    }
                    foreach (var item in c.GetSubjects())
                    {
                        vcdvm.Subjects.Add(new ClassSubject
                        {
                            Id = item.SubjectId,
                            Name = item.Name
                        });
                    }
                }
                ViewBag.Success = true;
                return View("ViewClassDetails", vcdvm);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        /// <summary>
        /// Action to View Details of a certain class
        /// </summary>
        /// <param name="id">class id</param>
        /// <param name="s">success flag</param>
        /// <param name="err">error flag</param>
        /// <returns></returns>
        public ActionResult ViewClassDetails(int id, bool s = false, bool err = false)
        {
            try
            {
                ViewBag.Success = s;
                ViewBag.Error = err;
                int absentToday = 0, presentToday = 0;
                HttpCookie conString = Request.Cookies.Get("rwxgqlb");
                Class c = new Class(id, Cryptography.Decrypt(conString.Value));
                ViewClassDetailsViewModel vcdvm = new ViewClassDetailsViewModel
                {
                    Id = c.ClassId,
                    Incharge = c.Incharge.Name,
                    Name = c.Name,
                    RollNo = c.RollNoIndex,
                    Sections = new List<ClassSection>(),
                    Strength = c.Strength,
                    Subjects = new List<ClassSubject>(),
                    Progress = new List<SectionMonthlyProgress>()
                };
                foreach (var item in c.GetSections())
                {
                    vcdvm.Sections.Add(new ClassSection
                    {
                        Id = item.SectionId,
                        Name = item.Name,
                        Strength = item.Strength
                    });
                    absentToday += item.GetAbsentStudents(DateTime.Now);
                    presentToday += item.GetPresentStudents(DateTime.Now);
                }
                foreach (var item in c.GetSubjects())
                {
                    vcdvm.Subjects.Add(new ClassSubject
                    {
                        Id = item.SubjectId,
                        Name = item.Name
                    });
                }
                for (int i = 1; i <= 12; i++)
                {
                    Random ran = new Random();
                    Month month = new Month { Number = i, Year = DateTime.Now.Year };
                    List<SectionProgress> lstSecProgress = new List<SectionProgress>();
                    foreach (var item in c.GetSections())
                    {
                        lstSecProgress.Add(new SectionProgress
                        {
                            ProgressPercent = item.GetMonthlyProgress(month).Result,
                            SectionName = item.Name
                        });
                    }
                    vcdvm.Progress.Add(new SectionMonthlyProgress
                    {
                        Month = month.Number,
                        SectionsProgres = lstSecProgress
                    });
                }
                return View(vcdvm);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        /// <summary>
        /// Action to remove a subject from  the class
        /// </summary>
        /// <param name="id">subject Id</param>
        /// <returns></returns>
        public ActionResult RemoveSubject(int id)
        {
            try
            {
                HttpCookie conString = Request.Cookies.Get("rwxgqlb");
                Subject s = new Subject(id, Cryptography.Decrypt(conString.Value));
                s.Delete();
                return RedirectToAction("ViewClassDetails", new { id = s.Class.ClassId, s = true });
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        /// <summary>
        /// Action to remove section of a class
        /// </summary>
        /// <param name="id">Section Id</param>
        /// <returns></returns>
        public ActionResult RemoveSection(int id)
        {
            try
            {
                HttpCookie conString = Request.Cookies.Get("rwxgqlb");
                Section s = new Section(id, Cryptography.Decrypt(conString.Value));
                if (s.Strength != 0)
                {
                    return RedirectToAction("ViewClassDetails", new { id = s.Class.ClassId, err = true });
                }
                else
                {
                    s.Delete();
                    return RedirectToAction("ViewClassDetails", new { id = s.Class.ClassId, s = true });
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        /// <summary>
        /// Action to load form to change the incharge of a particular class
        /// </summary>
        /// <param name="id">Class IIncharge Id</param>
        /// <returns></returns>
        public ActionResult ChangeClassIncharge(int id)
        {
            try
            {
                ViewBag.ClassId = id;
                return View();
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        /// <summary>
        /// Post Method to get form data to change class incharge
        /// </summary>
        /// <param name="model">form data</param>
        /// <param name="id">class id</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeClassIncharge(ChangeClassInchargeViewModel model, int id)
        {
            try
            {
                ViewBag.ClassId = id;
                if (!ModelState.IsValid)
                {
                    return View();
                }
                HttpCookie conString = Request.Cookies.Get("rwxgqlb");
                Class c = new Class(id, Cryptography.Decrypt(conString.Value));
                try
                {
                    c.Incharge = new Teacher(model.TeacherId, Cryptography.Decrypt(conString.Value));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
                return RedirectToAction("ViewClassDetails", new { s = true });
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        /// <summary>
        /// Action to add section for a certain class
        /// </summary>
        /// <param name="id">class id</param>
        /// <returns></returns>
        public ActionResult AddSection(int id)
        {
            try
            {
                ViewBag.ClassId = id;
                return View();
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        /// <summary>
        /// Post Method to get form data and add section to the class
        /// </summary>
        /// <param name="model">form data</param>
        /// <param name="id">class id</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddSection(AddSectionViewModel model, int id)
        {
            try
            {
                ViewBag.ClassId = id;
                if (!ModelState.IsValid)
                {
                    return View();
                }
                HttpCookie conString = Request.Cookies.Get("rwxgqlb");
                try
                {
                    Section s = new Section(model.Name.ToUpper(), id, Cryptography.Decrypt(conString.Value));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                    return View();
                }
                return RedirectToAction("ViewClassDetails", new { id = id, s = true });
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        /// <summary>
        /// Action to return view to add a subject to the class
        /// </summary>
        /// <param name="id">class id</param>
        /// <returns></returns>
        public ActionResult AddSubject(int id)
        {
            try
            {
                ViewBag.ClassId = id;
                return View();
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        /// <summary>
        /// Post Method to Add Subject to a certain class
        /// </summary>
        /// <param name="model">form data</param>
        /// <param name="id">class id</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddSubject(AddSubjectViewModel model, int id)
        {
            try
            {
                ViewBag.ClassId = id;
                if (!ModelState.IsValid)
                {
                    return View();
                }
                HttpCookie conString = Request.Cookies.Get("rwxgqlb");
                try
                {
                    Class c = new Class(id, Cryptography.Decrypt(conString.Value));
                    c.AddSubjects(model.Name.ToUpper());
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                    return View();
                }
                return RedirectToAction("ViewClassDetails", new { id = id, s = true });
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        /// <summary>
        /// Action to view list of sections of a perticular class
        /// </summary>
        /// <param name="id">class id</param>
        /// <param name="s">success flag</param>
        /// <param name="err">error flag</param>
        /// <returns></returns>
        public ActionResult ViewSection(int id, bool s = false, bool err = false)
        {
            try
            {
                ViewBag.Success = s;
                ViewBag.Error = err;
                HttpCookie conString = Request.Cookies.Get("rwxgqlb");
                Section sec = new Section(id, Cryptography.Decrypt(conString.Value));
                List<SectionBrightStudent> lstBrightStudents = new List<SectionBrightStudent>();
                ViewSectionViewModel vsvm = new ViewSectionViewModel
                {
                    Class = sec.Class.Name,
                    Id = sec.SectionId,
                    Name = sec.Name,
                    BrightStudents = new List<SectionBrightStudent>(3),
                    StudentAbsent = sec.GetAbsentStudents(DateTime.Now),
                    StudentPresent = sec.GetPresentStudents(DateTime.Now),
                    AssignedTeachers = new List<SectionAssignedTeacher>()
                };
                foreach (var item in sec.GetAssingedTeachers())
                {
                    vsvm.AssignedTeachers.Add(new SectionAssignedTeacher
                    {
                        SubId = item.Subject.SubjectId,
                        SubName = item.Subject.Name,
                        TId = item.Teacher.StaffId,
                        TName = item.Teacher.Name
                    });
                }
                List<Subject> lstSubjects = sec.Class.GetSubjects();
                int assgCount = vsvm.AssignedTeachers.Count;
                foreach (var item in lstSubjects)
                {
                    bool flag = true;
                    if (assgCount != 0)
                    {
                        foreach (var item2 in vsvm.AssignedTeachers)
                        {
                            if (item.SubjectId == item2.SubId)
                                flag = false;
                        }
                    }
                    if (flag)
                    {
                        vsvm.AssignedTeachers.Add(new SectionAssignedTeacher
                        {
                            SubId = item.SubjectId,
                            SubName = item.Name,
                            TId = 0,
                            TName = "Unassigned"
                        });
                    }
                }
                foreach (var item in sec.GetStudents())
                {
                    Month m = new Month { Number = DateTime.Now.Month, Year = DateTime.Now.Year };
                    decimal monthlyProgress = item.GetMonthlyProgress(m);
                    decimal monthAttandance = (((DateTime.Now - new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1)).Days - item.GetAbsents(DateTime.Now)) / (DateTime.Now - new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1)).Days) * 100;
                    lstBrightStudents.Add(new SectionBrightStudent
                    {
                        Progress = monthlyProgress,
                        Attendance = monthAttandance,
                        Name = item.Name,
                        RollNumber = item.RollNumber
                    });
                }
                try
                {
                    lstBrightStudents.Sort();
                    lstBrightStudents.Reverse();
                }
                catch (InvalidOperationException)
                {

                }
                for (int i = 0; (i < 5 && i < lstBrightStudents.Count); i++)
                {
                    vsvm.BrightStudents.Add(lstBrightStudents[i]);
                }
                return View(vsvm);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        /// <summary>
        /// Action to view the list of students
        /// </summary>
        /// <param name="secId">Section Id of which the students belong to</param>
        /// <param name="page">page number</param>
        /// <param name="searchName">query to search the students by name</param>
        /// <param name="s">success flag</param>
        /// <param name="err">error flag</param>
        /// <returns></returns>
        public ActionResult ViewStudents(int? secId, int? page, string searchName = "", bool s = false, bool err = false)
        {
            try
            {
                ViewBag.Success = s;
                ViewBag.Error = err;
                ViewBag.SectionId = secId;
                HttpCookie conString = Request.Cookies.Get("rwxgqlb");
                List<ViewStudentsViewModel> lstStudents = new List<ViewStudentsViewModel>();
                if (secId != null)
                {
                    Section sec = new Section(secId ?? 0, Cryptography.Decrypt(conString.Value));
                    foreach (var item in sec.GetStudents())
                    {
                        if (item.Name.ToLower().Contains(searchName.ToLower()))
                        {
                            lstStudents.Add(new ViewStudentsViewModel
                            {
                                AddNmbr = item.AdmissionNumber,
                                CName = sec.Class.Name,
                                Name = item.Name,
                                FName = item.Parent.FatherName,
                                Id = item.StudentId,
                                ParentId = item.Parent.ParentId,
                                RollNumber = item.RollNumber,
                                Progress = decimal.ToInt32(item.GetMonthlyProgress(new Month { Number = DateTime.Now.Month, Year = DateTime.Now.Year }))
                            });
                        }
                    }
                }
                else
                {
                    foreach (var item in Student.Search(searchName, Cryptography.Decrypt(conString.Value)))
                    {
                        lstStudents.Add(new ViewStudentsViewModel
                        {
                            AddNmbr = item.AdmissionNumber,
                            CName = item.Section.Class.Name,
                            Name = item.Name,
                            FName = item.Parent.FatherName,
                            Id = item.StudentId,
                            ParentId = item.Parent.ParentId,
                            RollNumber = item.RollNumber,
                            Progress = decimal.ToInt32(item.GetMonthlyProgress(new Month { Number = DateTime.Now.Month, Year = DateTime.Now.Year }))
                        });
                    }
                }
                PagedList<ViewStudentsViewModel> model = new PagedList<ViewStudentsViewModel>(lstStudents, page ?? 1, 20);
                return View(model);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        /// <summary>
        /// Action to assign section to the teacher
        /// </summary>
        /// <param name="secId">unique id of section</param>
        /// <param name="subId">unique id of subject</param>
        /// <returns></returns>
        public ActionResult AssignSecTeacher(int secId, int subId)
        {
            ViewBag.SectionId = secId;
            ViewBag.SubjectId = subId;
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        /// <summary>
        /// Post method to implement Assignement of the section to a teacher
        /// </summary>
        /// <param name="model">form data</param>
        /// <param name="secId">section id</param>
        /// <param name="subId">subject id</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssignSecTeacher(AssignTeacherViewModel model, int secId, int subId)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.SectionId = secId;
                ViewBag.SubjectId = subId;
                return View();
            }
            try
            {
                HttpCookie conString = Request.Cookies.Get("rwxgqlb");
                Teacher t = new Teacher(model.TeacherId, Cryptography.Decrypt(conString.Value));
                try
                {
                    if (t.AssignSection(secId, subId))
                    {
                        return RedirectToAction("ViewSection", new { id = secId, s = true });
                    }
                    else
                    {
                        return RedirectToAction("ViewSection", new { id = secId, err = true });
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.SectionId = secId;
                    ViewBag.SubjectId = subId;
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
        /// Action to change Section of a teacher
        /// </summary>
        /// <param name="secId">section id</param>
        /// <param name="subId">subject id</param>
        /// <param name="tId">teacher id</param>
        /// <returns></returns>
        public ActionResult ChangeSecTeacher(int secId, int subId, int tId)
        {
            ViewBag.SectionId = secId;
            ViewBag.SubjectId = subId;
            ViewBag.TeacherId = tId;
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        /// <summary>
        /// Post method ot implement Changing of the section for teacher
        /// </summary>
        /// <param name="model">form data</param>
        /// <param name="secId">section id</param>
        /// <param name="subId">subject id</param>
        /// <param name="tId">teacher id</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeSecTeacher(AssignTeacherViewModel model, int secId, int subId, int tId)
        {
            ViewBag.TeacherId = tId;
            ViewBag.SectionId = secId;
            ViewBag.SubjectId = subId;
            if (!ModelState.IsValid)
            {
                return View();
            }
            try
            {
                HttpCookie conString = Request.Cookies.Get("rwxgqlb");
                Teacher prevTeacher = new Teacher(tId, Cryptography.Decrypt(conString.Value));
                Teacher newTeacher = new Teacher(model.TeacherId, Cryptography.Decrypt(conString.Value));
                foreach (var item in prevTeacher.GetAssignedSections())
                {
                    if (item.Section.SectionId == secId && item.Subject.SubjectId == subId)
                    {
                        if (!prevTeacher.RemoveSection(secId, subId))
                        {
                            return RedirectToAction("ViewSection", new { id = secId, err = true });
                        }
                    }
                }
                try
                {
                    if (newTeacher.AssignSection(secId, subId))
                    {
                        return RedirectToAction("ViewSection", new { id = secId, s = true });
                    }
                    else
                    {
                        return RedirectToAction("ViewSection", new { id = secId, err = true });
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.SectionId = secId;
                    ViewBag.SubjectId = subId;
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
        /// Action to view list of classes
        /// </summary>
        /// <param name="page">page number</param>
        /// <param name="s">success flag</param>
        /// <param name="err">error flag</param>
        /// <returns></returns>
        public ActionResult ViewClasses(int? page, bool s = false, bool err = false)
        {
            try
            {
                ViewBag.Success = s;
                ViewBag.Error = err;
                HttpCookie conString = Request.Cookies.Get("rwxgqlb");
                List<ViewClassesViewModel> lstClasses = new List<ViewClassesViewModel>();
                foreach (var item in Class.GetAllClasses(Cryptography.Decrypt(conString.Value)))
                {
                    int sectionCount = 0, strength = 0;
                    foreach (var item2 in item.GetSections())
                    {
                        strength += item2.Strength;
                        sectionCount++;
                    }
                    lstClasses.Add(new ViewClassesViewModel
                    {
                        Id = item.ClassId,
                        Name = item.Name,
                        Sections = sectionCount,
                        Strength = strength
                    });
                }
                if (lstClasses.Count == 0)
                {
                    ViewBag.Empty = true;
                }
                PagedList<ViewClassesViewModel> model = new PagedList<ViewClassesViewModel>(lstClasses, page ?? 1, 20);
                return View(model);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        /// <summary>
        /// Action to return the view to load students to promote according to the class
        /// </summary>
        /// <param name="s">success flag</param>
        /// <returns></returns>
        public ActionResult LoadStudentsToPromote(bool s = false)
        {
            ViewBag.Success = s;
            return View();
        }
        /// <summary>
        /// Post Method to load students to promote for a class
        /// </summary>
        /// <param name="model">form data</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LoadStudentsToPromote(LoadStudentsViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }
                return RedirectToAction("ViewStudentsToPromote", new { secId = model.Section });
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        /// <summary>
        /// Action which returns the list of loaded students to promote form LoadStudentsToPromote Action
        /// </summary>
        /// <param name="secId">section id</param>
        /// <param name="s">success flag</param>
        /// <param name="err">error flag</param>
        /// <returns></returns>
        public ActionResult ViewStudentsToPromote(int secId, bool s = false, bool err = false)
        {
            try
            {
                ViewBag.Success = s;
                ViewBag.Error = err;
                ViewBag.SectionId = secId;
                HttpCookie conString = Request.Cookies.Get("rwxgqlb");
                List<ViewStudentToPromoteViewModel> lstStudents = new List<ViewStudentToPromoteViewModel>();
                Section sec = new Section(secId, Cryptography.Decrypt(conString.Value));
                foreach (var item in sec.GetStudents())
                {
                    lstStudents.Add(new ViewStudentToPromoteViewModel
                    {
                        Id = item.StudentId,
                        Name = item.Name,
                        RollNo = item.RollNumber
                    });
                }
                return View(lstStudents);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        /// <summary>
        /// Post Method to implement Promote Students
        /// </summary>
        /// <param name="secId">unique section id</param>
        /// <param name="studentId">unique student id</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PromoteStudents(int secId, IEnumerable<int> studentId)
        {
            try
            {
                List<int> lstStudentId;
                if (studentId == null)
                {
                    return RedirectToAction("ViewStudentsToPromote", new { secId = secId, err = true });
                }
                else
                {
                    lstStudentId = studentId.ToList();
                }
                TempData.Add("StudentIdToPromote", lstStudentId);
                return RedirectToAction("PromoteToSection");
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        public ActionResult PromoteToSection()
        {
            return View();
        }
        /// <summary>
        /// Post Method that implements the promote students to a certain section
        /// </summary>
        /// <param name="model">form data</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PromoteToSection(LoadStudentsViewModel model)
        {
            try
            {
                HttpCookie conString = Request.Cookies.Get("rwxgqlb");
                List<int> lstStudents = (List<int>)TempData["StudentIdToPromote"];
                TempData.Remove("StudentIdToPromote");
                foreach (var id in lstStudents)
                {
                    Student s = new Student(id, Cryptography.Decrypt(conString.Value));
                    s.Section = new Section(model.Section, Cryptography.Decrypt(conString.Value));
                }
                return RedirectToAction("LoadStudentsToPromote", new { s = true });
            }
            catch (Exception)
            {
                return RedirectToAction("ViewStudentsToPromote", new { err = true });
            }
        }
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
        public ActionResult LoadStudentAttendanceFor()
        {
            return View();
        }
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
        public ActionResult LoadStaffAttendanceFor()
        {
            return View();
        }
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
        }/// <summary>
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