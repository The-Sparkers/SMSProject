using Microsoft.Office.Interop.Excel;
using PagedList;
using SMSProject.Models;
using SMSProject.ServiceModules;
using SMSProject.ViewModels.AdminViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
                catch (Exception ex)
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
        public ActionResult ViewItemDetails(int id, bool s = false)
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
        /// <summary>
        /// Action to View all inventory items present in the database
        /// </summary>
        /// <param name="page">page number for pagging</param>
        /// <returns></returns>
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
        /// <summary>
        /// Action to return items belong to the category
        /// </summary>
        /// <param name="id">unique id of the category</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult _FilterCategory(int id)
        {
            try
            {
                if (id == 0)
                {
                    return RedirectToAction("ViewItems");
                }
                HttpCookie conString = Request.Cookies.Get("rwxgqlb");
                List<ViewItemsViewModel> lstItems = new List<ViewItemsViewModel>();
                InventoryCategory category = new InventoryCategory(id, Cryptography.Decrypt(conString.Value));
                foreach (var item in category.GetItems())
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
                PagedList<ViewItemsViewModel> model = new PagedList<ViewItemsViewModel>(lstItems, 1, (lstItems.Count > 1) ? lstItems.Count : 1);
                return View("ViewItems", model);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        /// <summary>
        /// Action to load interface for selling inventory items.
        /// </summary>
        /// <returns></returns>
        public ActionResult SellItem() => View();
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SellItem(SellItemViewModel model)
        {
            try
            {
                HttpCookie conString = Request.Cookies.Get("rwxgqlb");
                foreach (var item in model.ListSellItems)
                {
                    try
                    {
                        InventoryItem i = new InventoryItem(item.Id, Cryptography.Decrypt(conString.Value));
                        i.ItemSell(model.Id, item.Quantity);
                    }
                    catch (Exception ex)
                    {
                        return Content(ex.Message);
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        public ActionResult PrintItemList()
        {
            try
            {
                HttpCookie schoolName = Request.Cookies.Get("schlNm");
                HttpCookie conString = Request.Cookies.Get("rwxgqlb");
                System.Data.DataTable itemTable = new System.Data.DataTable();
                itemTable.Columns.Add("Item_Code", typeof(int));
                itemTable.Columns.Add("Item_Name", typeof(string));
                itemTable.Columns.Add("Category", typeof(string));
                itemTable.Columns.Add("Quantity", typeof(int));
                itemTable.Columns.Add("Price", typeof(decimal));
                foreach (var category in InventoryCategory.GetAllCategories(Cryptography.Decrypt(conString.Value)))
                {
                    foreach (var item in category.GetItems())
                    {
                        itemTable.Rows.Add(item.ItemId, item.Name, item.Category.CategoryName, item.Quantity, item.Price);
                    }
                }
                //code to create excel file using interop.excel
                Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();
                Workbook excelWorkBook = excelApp.Workbooks.Add(XlWBATemplate.xlWBATWorksheet);

                Worksheet excelWorkSheet = excelWorkBook.Sheets.Add();
                excelWorkSheet.Name = Convert.ToString("Items List");
                excelWorkSheet.Columns.AutoFit();
                for (int i = 1; i < itemTable.Columns.Count + 1; i++)
                {
                    excelWorkSheet.Cells[1, i] = itemTable.Columns[i - 1].ColumnName;
                }
                for (int j = 0; j < itemTable.Rows.Count; j++)
                {
                    for (int k = 0; k < itemTable.Columns.Count; k++)
                    {
                        excelWorkSheet.Cells[j + 2, k + 1] = itemTable.Rows[j].ItemArray[k].ToString();
                    }
                }
                string dirName = Server.MapPath("~/ExportableResources/");
                if (!Directory.Exists(dirName))
                    Directory.CreateDirectory(dirName);
                string fileName = dirName + "export_items_" + schoolName.Value + ".xlsb";
                if (System.IO.File.Exists(fileName))
                {
                    System.IO.File.Delete(fileName);
                    excelWorkBook.SaveAs(fileName, XlFileFormat.xlExcel12, Type.Missing, Type.Missing, Type.Missing, Type.Missing, XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                }
                else
                {
                    excelWorkBook.SaveAs(fileName, XlFileFormat.xlExcel12, Type.Missing, Type.Missing, Type.Missing, Type.Missing, XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                }
                excelWorkBook.Close();
                excelApp.Quit();
                return File(fileName, "application / vnd.ms - excel", "items_list.xlsb");
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
    }
}