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
    public class InventoryController : Controller
    {
        /// <summary>
        /// Action to return View for Adding a new category
        /// </summary>
        /// <returns></returns>
        public ActionResult AddCategory() => View();

        /// <summary>
        /// Post method to implement Add Category process
        /// </summary>
        /// <param name="model">form data</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCategory(AddCategoryViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View();
                HttpCookie conString = Request.Cookies.Get("rwxgqlb");
                InventoryCategory category = null;
                try
                {
                    category = new InventoryCategory(model.Name, Cryptography.Decrypt(conString.Value));
                }
                catch (Exception ex)
                {
                    ViewBag.Success = false;
                    ModelState.AddModelError(String.Empty, ex.Message);
                    return View();
                }
                ViewBag.Success = true;
                return View();
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        /// <summary>
        /// Action to add new Item into the system
        /// </summary>
        /// <returns></returns>
        public ActionResult AddItem() => View();
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddItem(AddItemViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View();
                HttpCookie conString = Request.Cookies.Get("rwxgqlb");
                InventoryItem item = null;
                try
                {
                    item = new InventoryItem(model.Name, model.Quantity, model.Price, model.Category, Cryptography.Decrypt(conString.Value));
                }
                catch(Exception ex)
                {
                    ViewBag.Success = false;
                    ModelState.AddModelError(String.Empty, ex.Message);
                    return View();
                }
                return RedirectToAction("ViewItemDetails", new { id = item.ItemId });
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        public ActionResult ViewItemDetails(int id, bool s=false)
        {
            try
            {
                HttpCookie conString = Request.Cookies.Get("rwxgqlb");
                InventoryItem item = new InventoryItem(id, Cryptography.Decrypt(conString.Value));
                ViewItemDetailsViewModel model = new ViewItemDetailsViewModel
                {
                    Category = item.Category.CategoryName,
                    Id = item.ItemId,
                    Name = item.Name,
                    Price = item.Price,
                    Quantity = item.Quantity
                };
                return View(model);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        /// <summary>
        /// Method to Set new price for the item called by AJAX in the view
        /// </summary>
        /// <param name="id">unique id of the item</param>
        /// <param name="newPrice">price to which updated</param>
        /// <returns></returns>
        [HttpPost]
        public ContentResult UpdateItemPrice(int id, decimal newPrice)
        {
            try
            {
                HttpCookie conString = Request.Cookies.Get("rwxgqlb");
                InventoryItem item = new InventoryItem(id, Cryptography.Decrypt(conString.Value));
                item.Price = newPrice;
                return Content(decimal.Round(newPrice, 2).ToString());
            }
            catch (Exception)
            {
                return Content("Error");
            }
        }
        /// <summary>
        /// Method to Set new quantity for the item called by AJAX in the view
        /// </summary>
        /// <param name="id">unique id of the item</param>
        /// <param name="newPrice">price to which updated</param>
        /// <returns></returns>
        [HttpPost]
        public ContentResult UpdateItemQuantity(int id, int newQuantity)
        {
            try
            {
                HttpCookie conString = Request.Cookies.Get("rwxgqlb");
                InventoryItem item = new InventoryItem(id, Cryptography.Decrypt(conString.Value));
                item.Quantity = newQuantity;
                return Content(newQuantity.ToString());
            }
            catch (Exception)
            {
                return Content("Error");
            }
        }
        public ActionResult ViewItems(int? page)
        {
            try
            {
                HttpCookie conString = Request.Cookies.Get("rwxgqlb");
                List<ViewItemsViewModel> lstItems = new List<ViewItemsViewModel>();
                foreach (var item in InventoryItem.GetAllItems(Cryptography.Decrypt(conString.Value)))
                {
                    lstItems.Add(new ViewItemsViewModel
                    {
                        Category = item.Category.CategoryName,
                        Id = item.ItemId,
                        Name = item.Name,
                        Price = item.Price,
                        Quantity = item.Quantity
                    });
                }
                PagedList<ViewItemsViewModel> model = new PagedList<ViewItemsViewModel>(lstItems, page ?? 1, 20);
                return View(model);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
    }
}