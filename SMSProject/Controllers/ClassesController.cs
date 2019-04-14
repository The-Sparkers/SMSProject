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
    public class ClassesController : Controller
    {

        /// <summary>
        /// Action to return the add class view
        /// </summary>
        /// <returns></returns>
        public ActionResult AddClass() => View();
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
    }
}