using PagedList;
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
    public class StudentsController : Controller
    {
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
    }
}