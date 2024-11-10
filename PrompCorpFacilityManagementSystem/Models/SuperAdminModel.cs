using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PrompCorpFacilityManagementSystem.Models
{
    public class Assets
    {
        public int AssetID { get; set; }
        public int SupplierID { get; set; }
        public string AssetName { get; set; }
        public string AssetDescription { get; set; }
        public string AssetType { get; set; }
        public string PurchasedDate { get; set; }
        public decimal Price { get; set; }
        public string AssetStatus { get; set; }
        public int AssetStatusID { get; set; }
        public string Category { get; set; }
        public string Brand { get; set; }
        public string SupplierName { get; set; }
        public string AssetImage { get; set; }
        public int CategoryID { get; set; }
        public int BrandID { get; set; }
        public int LocationID { get; set; }
        public string BrandName { get; set; }
        public string CategoryName { get; set; }
    }
    public class Suppliers
    {
        public int SupplierID { get; set; }
        public string SupplierName { get; set; }
        public string ContactName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string CityName { get; set; }
        public string CountryName { get; set; }
        public string SupplierStatus { get; set; }
        public int TotalItemsPurchased { get; set; }
        public string SupplierImage { get; set; }
        public string CreatedDate { get; set; }
        public int CityID { get; set; }
        public int CountryID { get; set; }
        public int Status { get; set; }
    }
    public class SuperAdminModel
    {
        public List<Technicians> Technicians { get; set; }
        public List<SelectListItem> lTechnicians { get; set; }
        public List<SelectListItem> getTechnicians()
        {
            List<SelectListItem> ret = new List<SelectListItem>();
            if (Technicians != null)
            {
                for (int i = 0; i < Technicians.Count(); i++)
                    ret.Add(new SelectListItem() { Text = Technicians[i].TechnicianName, Value = Technicians[i].TechnicianID.ToString() });
                return ret;
            }
            else return null;
        }
        public List<MaintenanceRecord> MaintenanceRecord = new List<MaintenanceRecord>();
        public List<AssetIssuedHistory> AssetIssuedHistory = new List<AssetIssuedHistory>();
        public List<Assets> Assets = new List<Assets>();
        public List<Suppliers> Suppliers = new List<Suppliers>();
        public List<SupplierOdooDocuments> SupplierOdooDocuments = new List<SupplierOdooDocuments>();
        public List<Suppliers> SuppliersList = new List<Suppliers>();
        public List<Country> Countries { get; set; }
        public List<SelectListItem> lCountries { get; set; }
        internal List<SelectListItem> getCountries()
        {
            List<SelectListItem> ret = new List<SelectListItem>();
            if (Countries != null)
            {
                for (int i = 0; i < Countries.Count(); i++)
                    ret.Add(new SelectListItem() { Text = Countries[i].CountryName, Value = Countries[i].CountryID.ToString() });
                return ret;
            }
            else return null;
        }
        public List<Category> Categories { get; set; }
        public List<SelectListItem> lCategories { get; set; }
        internal List<SelectListItem> getCategories()
        {
            List<SelectListItem> ret = new List<SelectListItem>();
            if (Categories != null)
            {
                for (int i = 0; i < Categories.Count(); i++)
                    ret.Add(new SelectListItem() { Text = Categories[i].CategoryName, Value = Categories[i].CategoryID.ToString() });
                return ret;
            }
            else return null;
        }

        public List<Brand> Brands { get; set; }
        public List<SelectListItem> lBrands { get; set; }
        internal List<SelectListItem> getBrands()
        {
            List<SelectListItem> ret = new List<SelectListItem>();
            if (Brands != null)
            {
                for (int i = 0; i < Brands.Count(); i++)
                    ret.Add(new SelectListItem() { Text = Brands[i].BrandName, Value = Brands[i].BrandID.ToString() });
                return ret;
            }
            else return null;
        }
        public List<Location> Locations { get; set; }
        public List<SelectListItem> lLocations { get; set; }
        internal List<SelectListItem> getLocations()
        {
            List<SelectListItem> ret = new List<SelectListItem>();
            if (Locations != null)
            {
                for (int i = 0; i < Locations.Count(); i++)
                    ret.Add(new SelectListItem()
                    {
                        Text = Locations[i].BuildingName + " - " + Locations[i].CityName + ", " + Locations[i].CountryName + " - " + Locations[i].PostalCode,
                        Value = Locations[i].LocationID.ToString()
                    });
                return ret;
            }
            else return null;
        }
        public List<SelectListItem> lSuppliers { get; set; }
        internal List<SelectListItem> getSuppliers()
        {
            List<SelectListItem> ret = new List<SelectListItem>();
            if (SuppliersList != null)
            {
                for (int i = 0; i < SuppliersList.Count(); i++)
                    ret.Add(new SelectListItem()
                    {
                        Text = SuppliersList[i].SupplierName,
                        Value = SuppliersList[i].SupplierID.ToString()
                    });
                return ret;
            }
            else return null;
        }
        public List<City> Cities { get; set; }
        public List<SelectListItem> lCities { get; set; }
        internal List<SelectListItem> getCities()
        {
            List<SelectListItem> ret = new List<SelectListItem>();
            if (Cities != null)
            {
                for (int i = 0; i < Cities.Count(); i++)
                    ret.Add(new SelectListItem() { Text = Cities[i].CityName, Value = Cities[i].CityID.ToString() });
                return ret;
            }
            else return null;
        }
    }
    public class Country
    {
        public int CountryID { get; set; }
        public string CountryName { get; set; }
    }
    public class City
    {
        public int CountryID { get; set; }
        public int CityID { get; set; }
        public string CityName { get; set; }
    }
    public class Category
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
    }
    public class Brand
    {
        public int BrandID { get; set; }
        public string BrandName { get; set; }
    }
    public class Location
    {
        public int LocationID { get; set; }
        public string BuildingName { get; set; }
        public string CityName { get; set; }
        public string CountryName { get; set; }
        public string PostalCode { get; set; }
    }
    public class MaintenanceRecord
    {
        public int AssetID { get; set; }
        public string MaintenanceDate { get; set; }
        public string Description { get; set; }
        public string TechnicianName { get; set; }
        public string NextMaintenanceDate { get; set; }
    }
    public class AssetIssuedHistory
    {
        public int AssetID { get; set; }
        public int LocationID { get; set; }
        public string BuildingName { get; set; }
        public string IssuedOn { get; set; }
        public string IssuedByName { get; set; }
        public string ReleasedOn { get; set; }
        public string ReleasedByName { get; set; }
    }
    public class Technicians
    {
        public int TechnicianID { get; set; }
        public string TechnicianName { get; set; }
    }
    public class DocumentModel
    {
        public string FileName { get; set; }
        public string FileData { get; set; } // You may want to handle this differently (e.g., store as byte array)
    }
    public class SupplierOdooDocuments
    {
        public int SupplieriD { get; set; }
        public string FileName { get; set; }
        public string UploadedBy { get; set; }
        public string UploadedOn { get; set; }
    }
}