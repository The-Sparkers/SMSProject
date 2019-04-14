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
    public class FinanceController : Controller
    {

        /// <summary>
        /// Action to load view for set salaries
        /// </summary>
        /// <returns></returns>
        public ActionResult LoadSetSalaries() => View();
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
    }
}