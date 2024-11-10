using PrompCorpFacilityManagementSystem.DBFunctions;
using PrompCorpFacilityManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PrompCorpFacilityManagementSystem.Controllers
{
    public class SuperAdminController : Controller
    {
        // GET: SuperAdmin
        SuperAdminModel SAM = new SuperAdminModel();
        SuperAdmin SADB = new SuperAdmin();
        public ActionResult Index()
        {
            SAM.Assets = SADB.GetAssets();
            SAM.Suppliers = SADB.GetSuppliers();
            return View(SAM);
        }
        public ActionResult AddAsset(int id = 0)
        {
            SAM.Countries = SADB.GetCountries();
            SAM.lCountries = SAM.getCountries();
            SAM.Categories = SADB.GetCategories();
            SAM.lCategories = SAM.getCategories();
            SAM.Brands = SADB.GetBrands();
            SAM.lBrands = SAM.getBrands();
            SAM.Locations = SADB.GetLocations();
            SAM.lLocations = SAM.getLocations();
            SAM.SuppliersList = SADB.GetSuppliersList();
            SAM.lSuppliers = SAM.getSuppliers();
            if (id != 0) ViewData["AssetID"] = id.ToString();
            return View(SAM);
        }
        public ActionResult AddSupplier(int id = 0)
        {
            SAM.Countries = SADB.GetCountries();
            SAM.lCountries = SAM.getCountries();

            if (id != 0) ViewData["SupplierId"] = id.ToString();
            return View(SAM);
        }
        public ActionResult GetSpecificSupplier(int id)
        {
            List<Suppliers> r = new List<Suppliers>();
            r = SADB.GetSpecificSupplier(id);
            return Json(r, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetCountryCities(string CountryID)
        {

            return Json(SADB.GetCountryCities(CountryID).Tables[0].AsEnumerable().Select(x => new City
            {
                CityID = x.Field<int>("CityID"),
                CityName = x.Field<string>("CityName")
            }).ToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult AddEditSupplier(int SupplierID, string SupplierName, string ContactName, string Phone, string Email, string Address,
           string CityID, string CountryID, int Status,
           string SupplierImage)
        {
            try
            {

                int u = SADB.AddEditSupplier(SupplierID, SupplierName, ContactName, Phone, Email, Address, CityID,
                     CountryID, Status, SupplierImage);
                return Content(u.ToString());
            }
            catch (Exception ex)
            {

                return Content("Some Error Occurred ..");
            }
        }
        public ActionResult GetSpecificAsset(int id)
        {
            List<Assets> r = new List<Assets>();
            r = SADB.GetSpecificAsset(id);
            return Json(r, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AddEditAsset(int AssetID, string Name, string Description, string AssetType, string PurchaseDate, string Price, string AssetStatus,
          string CategoryID, string BrandID, string LocationID,
          string AssetImage, string SupplierID)
        {
            try
            {

                int u = SADB.AddEditAsset(AssetID, Name, Description, AssetType, PurchaseDate, Price, LocationID.Length > 0 ? "2" : "1",
                     CategoryID, BrandID, LocationID, AssetImage, SupplierID);
                return Content(u.ToString());
            }
            catch (Exception ex)
            {

                return Content("Some Error Occurred ..");
            }
        }
        public ActionResult SupplierDetail(string id)
        {
            SAM.Suppliers = SADB.GetSpecificSupplier(int.Parse(id));
            SAM.Assets = SADB.GetAssets(int.Parse(id));
            SAM.SupplierOdooDocuments = SADB.GetOdooDocuments(id);
            return View(SAM);
        }
        public ActionResult UploadSupplierLogo()
        {
            if (Request.Files.Count > 0)
            {
                try
                {
                    string fileName = "";

                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFileBase file = files[i];
                        string fname = "";

                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                            fileName = fname;
                        }
                        else
                        {
                            fname = file.FileName;
                            fileName = fname;
                        }
                        fileName = System.IO.Path.GetFileNameWithoutExtension(file.FileName);
                        if (fileName.Length > 30)
                        {
                            fileName = fileName.Substring(0, 30);
                        }
                        fileName.Replace(",", "_");
                        string extention = System.IO.Path.GetExtension(file.FileName);
                        fileName = fileName + DateTime.Now.ToFileTimeUtc() + "" + extention;
                        file.SaveAs(Path.Combine(Server.MapPath("~/images/suppliers/ "), fileName));
                    }
                    return Json(fileName);
                }
                catch (Exception ex)
                {
                    return Json("Error occurred. Error details: " + ex.Message);
                }
            }
            else
            {
                return Json("No files selected.");
            }
        }
        
        public ActionResult UploadAssetLogo()
        {
            if (Request.Files.Count > 0)
            {
                try
                {
                    string fileName = "";

                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFileBase file = files[i];
                        string fname = "";

                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                            fileName = fname;
                        }
                        else
                        {
                            fname = file.FileName;
                            fileName = fname;
                        }
                        fileName = System.IO.Path.GetFileNameWithoutExtension(file.FileName);
                        if (fileName.Length > 30)
                        {
                            fileName = fileName.Substring(0, 30);
                        }
                        fileName.Replace(",", "_");
                        string extention = System.IO.Path.GetExtension(file.FileName);
                        fileName = fileName + DateTime.Now.ToFileTimeUtc() + "" + extention;
                        file.SaveAs(Path.Combine(Server.MapPath("~/images/assets/ "), fileName));
                    }
                    return Json(fileName);
                }
                catch (Exception ex)
                {
                    return Json("Error occurred. Error details: " + ex.Message);
                }
            }
            else
            {
                return Json("No files selected.");
            }
        }
        public ActionResult AssetDetail(string id)
        {
            SAM.Assets = SADB.GetSpecificAsset(int.Parse(id));
            SAM.MaintenanceRecord = SADB.GetAssetMaintenanceHistory(int.Parse(id));
            SAM.AssetIssuedHistory = SADB.GetAssetIssuedHistory(int.Parse(id));
            SAM.Suppliers = SADB.GetSpecificSupplierForAsset(SAM.Assets[0].SupplierID);
            SAM.Technicians = SADB.GetTechnicians();
            SAM.lTechnicians = SAM.getTechnicians();
            SAM.Locations = SADB.GetLocations();
            SAM.lLocations = SAM.getLocations();
            return View(SAM);
        }
        [ValidateInput(false)]
        public ActionResult InserNewMaintenanceRecord(string AssetID, string MaintenanceDate, string Description,
            string TechnicianID,
            string NextMaintenanceDate)
        {

            try
            {
                return Content(SADB.InserNewMaintenanceRecord(AssetID, MaintenanceDate, Description, TechnicianID, NextMaintenanceDate).ToString());
            }
            catch (Exception ex)
            {

                return Content("0");
            }
        }
        public ActionResult InsertIssueRecord(string AsseteID,string LocationID,string IssueDate)
        {
            try
            {
                SADB.InsertIssueRecord(AsseteID, LocationID, IssueDate);
                return Content("1");
            }
            catch (Exception ex)
            {

                return Content("0");
            }
        }
        public ActionResult ReleaseAsset(string AssetID)
        {
            return Content(SADB.ReleaseAsset(AssetID).ToString());
        }
        public ActionResult DownloadFile(string fileName)
        {
            string filePath = Server.MapPath("~/images/OdooDocuments/" + fileName);
            if (System.IO.File.Exists(filePath))
            {
                byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            else
            {
                return HttpNotFound("File not found");
            }
        }
    }
}