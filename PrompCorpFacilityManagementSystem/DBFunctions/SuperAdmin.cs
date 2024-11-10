using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Data;
using PrompCorpFacilityManagementSystem.Models;

namespace PrompCorpFacilityManagementSystem.DBFunctions
{
    public class SuperAdmin
    {
        Sql sql = new Sql();
        internal List<AssetIssuedHistory> GetAssetIssuedHistory(int AssetID)
        {
            SqlCommand cmd = new SqlCommand(@"select IssueID,CONVERT(varchar,IssuedOn,106)IssuedOn,u.UserName IssuedBy,
CONVERT(varchar,ReleasedOn,106)ReleasedOn, ur.Username ReleasedBy,
l.BuildingName from tblAssetIssuedhistory h
INNER JOIN tblAssetLocations l on l.LocationID=h.LocationID
INNER JOIN tblUsers u on u.UserID=h.IssuedBy
LEFT OUTER JOIN tblUsers ur on ur.UserID=h.ReleasedBy
where h.AssetID=@AssetID order by h.IssueID DESC");
            cmd.Parameters.AddWithValue("@AssetID", AssetID);
            DataSet ds = sql.GetTableCMD(cmd, "MainDB");
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].AsEnumerable().Select(r => new AssetIssuedHistory
                {
                    BuildingName = r.Field<string>("BuildingName"),
                    IssuedOn = r.Field<string>("IssuedOn"),
                    IssuedByName = r.Field<string>("IssuedBy"),
                    ReleasedOn = r.Field<string>("ReleasedOn"),
                    ReleasedByName = r.Field<string>("ReleasedBy")
                }).ToList();
            else return null;
        }
        internal List<MaintenanceRecord> GetAssetMaintenanceHistory(int AssetID)
        {
            SqlCommand cmd = new SqlCommand(@"select MaintenanceID, convert(varchar,MaintenanceDate,106)MaintenanceDate,Description,
u.UserName,convert(varchar,NextMaintenanceDate,106) NextMaintenanceDate from tblAssetMaintenanceRecords r
INNER JOIN tblUsers u on u.UserID=r.TechnicianID
where AssetID=@AssetID order by MaintenanceID DESC");
            cmd.Parameters.AddWithValue("@AssetID", AssetID);
            DataSet ds = sql.GetTableCMD(cmd, "MainDB");
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].AsEnumerable().Select(r => new MaintenanceRecord
                {
                    MaintenanceDate = r.Field<string>("MaintenanceDate"),
                    Description = r.Field<string>("Description"),
                    TechnicianName = r.Field<string>("UserName"),
                    NextMaintenanceDate = r.Field<string>("NextMaintenanceDate")
                }).ToList();
            else return null;
        }
        internal List<Assets> GetSpecificAsset(int AssetID)
        {
            SqlCommand cmd = new SqlCommand(@"select a.AssetID, Name,Description,AssetType,convert(varchar,PurchaseDate,106)PurchaseDate,Price,
a.CategoryID,a.BrandID,LocationID,AssetImage,b.BrandName,c.CategoryName,a.AssetStatus,
assup.SupplierID from tblAsset a
INNER JOIN tblAssetSupplier assup on assup.AssetID=a.AssetID
INNER JOIN tblBrand b on b.BrandID=a.BrandID
INNER JOIN tblCategory c on c.CategoryID=a.CategoryID
where a.AssetID=@AssetID");
            cmd.Parameters.AddWithValue("@AssetID", AssetID);
            DataSet ds = sql.GetTableCMD(cmd, "MainDB");
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].AsEnumerable().Select(r => new Assets
                {
                    AssetID = r.Field<int>("AssetID"),
                    AssetName = r.Field<string>("Name"),
                    AssetDescription = r.Field<string>("Description"),
                    AssetType = r.Field<string>("AssetType"),
                    PurchasedDate = r.Field<string>("PurchaseDate"),
                    Price = r.Field<decimal>("Price"),
                    CategoryID = r.Field<int>("CategoryID"),
                    BrandID = r.Field<int>("BrandID"),
                    LocationID = r.Field<int>("LocationID"),
                    AssetImage = r.Field<string>("AssetImage"),
                    SupplierID = r.Field<int>("SupplierID"),
                    BrandName = r.Field<string>("BrandName"),
                    CategoryName = r.Field<string>("CategoryName"),
                    AssetStatusID = r.Field<int>("AssetStatus")
                }).ToList();
            else return null;
        }
        internal List<Suppliers> GetSpecificSupplier(int SupplierID)
        {
            SqlCommand cmd = new SqlCommand(@"select SupplierID, SupplierName,ContactName,Phone,Email,Address,s.CityID,s.CountryID,Status,SupplierImage,Convert(varchar, CreatedDate,106)CreatedDate,
ci.CityName,co.CountryName
from tblSupplier s
INNER JOIN tblCity ci on ci.CityID=s.CityID
INNER JOIN tblCountry co on co.CountryID=s.CountryID
            where SupplierID=@SupplierID");
            cmd.Parameters.AddWithValue("@SupplierID", SupplierID);
            DataSet ds = sql.GetTableCMD(cmd, "MainDB");
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].AsEnumerable().Select(r => new Suppliers
                {
                    SupplierID = r.Field<int>("SupplierID"),
                    SupplierName = r.Field<string>("SupplierName"),
                    ContactName = r.Field<string>("ContactName"),
                    Phone = r.Field<string>("Phone"),
                    Email = r.Field<string>("Email"),
                    Address = r.Field<string>("Address"),
                    CityID = r.Field<int>("CityID"),
                    CountryID = r.Field<int>("CountryID"),
                    Status = r.Field<int>("Status"),
                    SupplierImage = r.Field<string>("SupplierImage"),
                    CreatedDate = r.Field<string>("CreatedDate"),
                    CityName = r.Field<string>("CityName"),
                    CountryName = r.Field<string>("CountryName")
                }).ToList();
            else return null;
        }
        internal List<Suppliers> GetSpecificSupplierForAsset(int SupplierID)
        {
            SqlCommand cmd = new SqlCommand(@"select s.SupplierID, SupplierName,ContactName,Phone,Email,Address,s.CityID,s.CountryID,Status,SupplierImage,Convert(varchar, CreatedDate,106)CreatedDate,
ci.CityName,co.CountryName,
COUNT(a.AssetID) AS TotalItemsPurchased
from tblSupplier s
INNER JOIN tblCity ci on ci.CityID=s.CityID
INNER JOIN tblCountry co on co.CountryID=s.CountryID
LEFT JOIN 
    tblAssetSupplier a ON s.SupplierID = a.SupplierID
            where s.SupplierID=@SupplierID
            GROUP BY s.SupplierID,
    s.SupplierName, 
    s.ContactName, 
    s.CityID,
    s.CountryID,
    ci.CityName,co.CountryName,
    s.Phone, 
    s.Email, 
    s.Address,
    co.CountryName, 
    s.Status, 
    s.SupplierImage, 
    s.CreatedDate ");
            cmd.Parameters.AddWithValue("@SupplierID", SupplierID);
            DataSet ds = sql.GetTableCMD(cmd, "MainDB");
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].AsEnumerable().Select(r => new Suppliers
                {
                    SupplierID = r.Field<int>("SupplierID"),
                    SupplierName = r.Field<string>("SupplierName"),
                    ContactName = r.Field<string>("ContactName"),
                    Phone = r.Field<string>("Phone"),
                    Email = r.Field<string>("Email"),
                    Address = r.Field<string>("Address"),
                    CityID = r.Field<int>("CityID"),
                    CountryID = r.Field<int>("CountryID"),
                    Status = r.Field<int>("Status"),
                    SupplierImage = r.Field<string>("SupplierImage"),
                    CreatedDate = r.Field<string>("CreatedDate"),
                    CityName = r.Field<string>("CityName"),
                    CountryName = r.Field<string>("CountryName"),
                    TotalItemsPurchased = r.Field<int>("TotalItemsPurchased")
                }).ToList();
            else return null;
        }
        internal List<Suppliers> GetSuppliers()
        {
            SqlCommand cmd = new SqlCommand(@"SELECT s.SupplierID,
    s.SupplierName, 
    s.ContactName, 
    s.Phone, 
    s.Email, 
    s.Address, 
    c.CityName, 
    co.CountryName,
    CASE 
        WHEN s.Status = 1 THEN 'Active' 
        ELSE 'Inactive' 
    END AS SupplierStatus, 
    COUNT(a.AssetID) AS TotalItemsPurchased, 
    s.SupplierImage, 
   convert(varchar, s.CreatedDate,106)CreatedDate
FROM 
    tblSupplier s
LEFT JOIN 
    tblAssetSupplier a ON s.SupplierID = a.SupplierID
LEFT JOIN 
    tblCity c ON s.CityID = c.CityID
LEFT JOIN 
    tblCountry co ON c.CountryID = co.CountryID
GROUP BY s.SupplierID,
    s.SupplierName, 
    s.ContactName, 
    s.Phone, 
    s.Email, 
    s.Address, 
    c.CityName, 
    co.CountryName, 
    s.Status, 
    s.SupplierImage, 
    s.CreatedDate order by s.SupplierID DESC;");
            DataSet ds = sql.GetTableCMD(cmd, "MainDB");
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].AsEnumerable().Select(r => new Suppliers
                {
                    SupplierID = r.Field<int>("SupplierID"),
                    SupplierName = r.Field<string>("SupplierName"),
                    ContactName = r.Field<string>("ContactName"),
                    Phone = r.Field<string>("Phone"),
                    Email = r.Field<string>("Email"),
                    Address = r.Field<string>("Address"),
                    CityName = r.Field<string>("CityName"),
                    CountryName = r.Field<string>("CountryName"),
                    SupplierStatus = r.Field<string>("SupplierStatus"),
                    TotalItemsPurchased = r.Field<int>("TotalItemsPurchased"),
                    SupplierImage = r.Field<string>("SupplierImage"),
                    CreatedDate = r.Field<string>("CreatedDate")
                }).ToList();
            else return null;
        }
        internal List<Assets> GetAssets(int supplierid = 0)
        {
            SqlCommand cmd;
            if (supplierid == 0)
                cmd = new SqlCommand(@"SELECT a.AssetID,asup.SupplierID,
                                                a.Name,
                                                a.Description,
                                                a.AssetType,
                                                CONVERT(varchar, a.PurchaseDate, 106) AS PurchasedDate,
                                                a.Price,
                                                CASE 
                                                    WHEN a.AssetStatus = 1 THEN 'Available' 
                                                    WHEN a.AssetStatus = 2 THEN 'Issued' 
                                                    ELSE 'Not Available' 
                                                END AS AssetStatus,a.AssetStatus as AssetStatusID,
                                                c.CategoryName AS Category,
                                                b.BrandName AS Brand,
                                                s.SupplierName,a.AssetImage
                                                FROM 
                                                    tblAsset a 
                                                INNER JOIN 
                                                    tblCategory c ON c.CategoryID = a.CategoryID
                                                INNER JOIN 
                                                    tblBrand b ON b.BrandID = a.BrandID
                                                INNER JOIN 
                                                	tblAssetSupplier asup on asup.AssetID=a.AssetID
                                                INNER JOIN tblSupplier s on s.SupplierID=asup.SupplierID
                                                ORDER BY 
                                                    a.AssetID DESC;");
            else
            {
                cmd = new SqlCommand(@"SELECT a.AssetID,asup.SupplierID,
                                                a.Name,
                                                a.Description,
                                                a.AssetType,
                                                CONVERT(varchar, a.PurchaseDate, 106) AS PurchasedDate,
                                                a.Price,
                                                CASE 
                                                    WHEN a.AssetStatus = 1 THEN 'Available' 
                                                    WHEN a.AssetStatus = 2 THEN 'Issued' 
                                                    ELSE 'Not Available' 
                                                END AS AssetStatus,a.AssetStatus as AssetStatusID,
                                                c.CategoryName AS Category,
                                                b.BrandName AS Brand,
                                                s.SupplierName,a.AssetImage
                                                FROM 
                                                    tblAsset a 
                                                INNER JOIN 
                                                    tblCategory c ON c.CategoryID = a.CategoryID
                                                INNER JOIN 
                                                    tblBrand b ON b.BrandID = a.BrandID
                                                INNER JOIN 
                                                	tblAssetSupplier asup on asup.AssetID=a.AssetID
                                                INNER JOIN tblSupplier s on s.SupplierID=asup.SupplierID
                                                where asup.SupplierID=@SupplierID
                                                ORDER BY 
                                                    a.AssetID DESC;");
                cmd.Parameters.AddWithValue("@SupplierID", supplierid);
            }
            DataSet ds = sql.GetTableCMD(cmd, "MainDB");
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].AsEnumerable().Select(r => new Assets
                {
                    AssetID = r.Field<int>("AssetID"),
                    SupplierID = r.Field<int>("SupplierID"),
                    AssetName = r.Field<string>("Name"),
                    AssetDescription = r.Field<string>("Description"),
                    AssetType = r.Field<string>("AssetType"),
                    PurchasedDate = r.Field<string>("PurchasedDate"),
                    Price = r.Field<decimal>("Price"),
                    AssetStatus = r.Field<string>("AssetStatus"),
                    Category = r.Field<string>("Category"),
                    Brand = r.Field<string>("Brand"),
                    SupplierName = r.Field<string>("SupplierName"),
                    AssetImage = r.Field<string>("AssetImage"),
                    AssetStatusID = r.Field<int>("AssetStatusID")
                }).ToList();
            else return null;
        }

        internal List<Country> GetCountries()
        {
            SqlCommand cmd = new SqlCommand("select CountryID,CountryName from tblCountry order by CountryName");
            DataSet ds = sql.GetTableCMD(cmd, "MainDB");
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].AsEnumerable().Select(c => new Country
                {
                    CountryID = c.Field<int>("CountryID"),
                    CountryName = c.Field<string>("CountryName")
                }).ToList();
            else return null;
        }
        internal DataSet GetCountryCities(string CountryID)
        {
            SqlCommand cmd = new SqlCommand("select CityID,CityName from tblCity where CountryID=@CountryID order by CityName");
            cmd.Parameters.AddWithValue("@CountryID", CountryID);
            return sql.GetTableCMD(cmd, "MainDB");
        }
        public int AddEditSupplier(int SupplierID, string SupplierName, string ContactName, string Phone, string Email, string Address,
           string CityID, string CountryID, int Status,
           string SupplierImage)
        {
            SqlCommand cmd;
            if (SupplierID == 0)
            {
                cmd = new SqlCommand(@"insert into tblSupplier (SupplierName,ContactName,Phone,Email,Address,
                CityID,CountryID,Status,CreatedDate,CreatedBy,SupplierImage) VALUES(@SupplierName,@ContactName,@Phone,@Email,@Address,
                @CityID,@CountryID,@Status,GETDATE(),@CreatedBy,@SupplierImage);SELECT scope_identity();");
            }
            else
            {
                cmd = new SqlCommand(@"update tblSupplier set SupplierName=@SupplierName,ContactName=@ContactName,Phone=@Phone,Email=@Email,
                Address=@Address,CityID=@CityID,CountryID=@CountryID,Status=@Status,UpdatedDate=GETDATE(),UpdatedBy=@CreatedBy,SupplierImage=@SupplierImage
                where SupplierID=@SupplierID");
                cmd.Parameters.AddWithValue("@SupplierID", SupplierID);
            }
            cmd.Parameters.AddWithValue("@SupplierName", SupplierName);
            cmd.Parameters.AddWithValue("@ContactName", ContactName);
            cmd.Parameters.AddWithValue("@Phone", Phone);
            cmd.Parameters.AddWithValue("@Email", Email);
            cmd.Parameters.AddWithValue("@Address", Address);
            cmd.Parameters.AddWithValue("@CityID", CityID);
            cmd.Parameters.AddWithValue("@CountryID", CountryID);
            cmd.Parameters.AddWithValue("@Status", Status);
            cmd.Parameters.AddWithValue("@CreatedBy", ReadCookie.GetCookieValue("UserID"));
            cmd.Parameters.AddWithValue("@SupplierImage", SupplierImage);
            if (SupplierID == 0)
            {

                SupplierID = sql.ExecuteSqlWithID(cmd, "MainDB");
                return SupplierID;
            }
            else { sql.ExecuteSql(cmd, "MainDB"); return 0; }
        }
        internal List<Category> GetCategories()
        {
            SqlCommand cmd = new SqlCommand("select CategoryID,CategoryName from tblCategory order by CategoryName");
            DataSet ds = sql.GetTableCMD(cmd, "MainDB");
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].AsEnumerable().Select(c => new Category
                {
                    CategoryID = c.Field<int>("CategoryID"),
                    CategoryName = c.Field<string>("CategoryName")
                }).ToList();
            else return null;
        }
        internal List<Brand> GetBrands()
        {
            SqlCommand cmd = new SqlCommand("select BrandID,BrandName from tblBrand order by BrandName");
            DataSet ds = sql.GetTableCMD(cmd, "MainDB");
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].AsEnumerable().Select(c => new Brand
                {
                    BrandID = c.Field<int>("BrandID"),
                    BrandName = c.Field<string>("BrandName")
                }).ToList();
            else return null;
        }
        internal List<Suppliers> GetSuppliersList()
        {
            SqlCommand cmd = new SqlCommand("select SupplierID,SupplierName from tblSupplier order by SupplierName");
            DataSet ds = sql.GetTableCMD(cmd, "MainDB");
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].AsEnumerable().Select(c => new Suppliers
                {
                    SupplierID = c.Field<int>("SupplierID"),
                    SupplierName = c.Field<string>("SupplierName")
                }).ToList();
            else return null;
        }
        internal List<Technicians> GetTechnicians()
        {
            SqlCommand cmd = new SqlCommand(@"select UserID,Username from tblUsers where Role='T' and Status=1 order by username");
            DataSet ds = sql.GetTableCMD(cmd, "MainDB");
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].AsEnumerable().Select(c => new Technicians
                {
                    TechnicianID = c.Field<int>("UserID"),
                    TechnicianName = c.Field<string>("Username")
                }).ToList();
            else return null;
        }
        internal List<Location> GetLocations()
        {
            SqlCommand cmd = new SqlCommand(@"select LocationID,BuildingName,c.CityName,co.CountryName,PostalCode from tblAssetLocations l
            INNER JOIN tblCity c on c.CityID=l.CityID
            INNER JOIN tblCountry co on co.CountryID=l.CountryID
            order by BuildingName");
            DataSet ds = sql.GetTableCMD(cmd, "MainDB");
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].AsEnumerable().Select(c => new Location
                {
                    LocationID = c.Field<int>("LocationID"),
                    BuildingName = c.Field<string>("BuildingName"),
                    CityName = c.Field<string>("CityName"),
                    CountryName = c.Field<string>("CountryName"),
                    PostalCode = c.Field<string>("PostalCode")
                }).ToList();
            else return null;
        }
        public int AddEditAsset(int AssetID, string Name, string Description, string AssetType, string PurchaseDate, string Price, string AssetStatus,
          string CategoryID, string BrandID, string LocationID,
          string AssetImage, string SupplierID)
        {
            SqlCommand cmd;
            if (AssetID == 0)
            {
                cmd = new SqlCommand(@"insert into tblAsset (Name,Description,AssetType,PurchaseDate,Price,
                AssetStatus,CategoryID,BrandID,CreatedDate,CreatedBy,LocationID,AssetImage) VALUES(@Name,@Description,@AssetType,@PurchaseDate,@Price,
                @AssetStatus,@CategoryID,@BrandID,GETDATE(),@CreatedBy,@LocationID,@AssetImage);SELECT scope_identity();");
            }
            else
            {
                cmd = new SqlCommand(@"update tblAsset set Name=@Name,Description=@Description,AssetType=@AssetType,PurchaseDate=@PurchaseDate,
                Price=@Price,AssetStatus=@AssetStatus,CategoryID=@CategoryID,BrandID=@BrandID,UpdatedDate=GETDATE(),UpdatedBy=@CreatedBy,LocationID=@LocationID,
                AssetImage=@AssetImage where AssetID=@AssetID");
                cmd.Parameters.AddWithValue("@AssetID", AssetID);
            }
            cmd.Parameters.AddWithValue("@Name", Name);
            cmd.Parameters.AddWithValue("@Description", Description);
            cmd.Parameters.AddWithValue("@AssetType", AssetType);
            cmd.Parameters.AddWithValue("@PurchaseDate", PurchaseDate);
            cmd.Parameters.AddWithValue("@Price", Price);
            cmd.Parameters.AddWithValue("@AssetStatus", AssetStatus);
            cmd.Parameters.AddWithValue("@CategoryID", CategoryID);
            cmd.Parameters.AddWithValue("@BrandID", BrandID);
            cmd.Parameters.AddWithValue("@CreatedBy", ReadCookie.GetCookieValue("UserID"));
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@AssetImage", AssetImage);
            if (AssetID == 0)
            {
                AssetID = sql.ExecuteSqlWithID(cmd, "MainDB");
                cmd = new SqlCommand("insert into tblAssetSupplier(AssetID,SupplierID) VALUES(@AssetID,@SupplierID)");
                cmd.Parameters.AddWithValue("@AssetID", AssetID);
                cmd.Parameters.AddWithValue("@SupplierID", SupplierID);
                sql.ExecuteSql(cmd, "MainDB");

            }
            else
            {
                sql.ExecuteSql(cmd, "MainDB");
                cmd = new SqlCommand("update tblAssetSupplier set SupplierID=@SupplierID where AssetID=@AssetID");
                cmd.Parameters.AddWithValue("@AssetID", AssetID);
                cmd.Parameters.AddWithValue("@SupplierID", SupplierID);
                sql.ExecuteSql(cmd, "MainDB");
            }
            if (LocationID.Length > 0)
            {
                cmd = new SqlCommand(@"IF EXISTS (SELECT 1 FROM tblAssetIssuedHistory WHERE AssetID = @AssetID AND ReleasedOn IS NULL)
                                            BEGIN
                                                UPDATE tblAssetIssuedHistory
                                                SET LocationID = @LocationID, IssuedBy=@IssuedBy
                                                WHERE AssetID = @AssetID;
                                            END
                                       ELSE
                                            BEGIN
                                                INSERT INTO tblAssetIssuedHistory (AssetID, LocationID, IssuedOn, IssuedBy)
                                                VALUES (@AssetID, @LocationID, GETDATE(), @IssuedBy);
                                            END;");
                cmd.Parameters.AddWithValue("@AssetID", AssetID);
                cmd.Parameters.AddWithValue("@LocationID", LocationID);
                cmd.Parameters.AddWithValue("@IssuedBy", ReadCookie.GetCookieValue("UserID"));
                sql.ExecuteSql(cmd, "MainDB");
            }
            return AssetID;
        }
        public int InserNewMaintenanceRecord(string AssetID, string MaintenanceDate, string Description,
            string TechnicianID,
            string NextMaintenanceDate)
        {
            SqlCommand cmd = new SqlCommand(@"insert into tblAssetMaintenanceRecords (AssetID,MaintenanceDate,Description,TechnicianID,NextMaintenanceDate)
                                            VALUES(@AssetID,@MaintenanceDate,@Description,@TechnicianID,@NextMaintenanceDate); SELECT scope_identity();");
            cmd.Parameters.AddWithValue("@AssetID", AssetID);
            cmd.Parameters.AddWithValue("@MaintenanceDate", MaintenanceDate);
            cmd.Parameters.AddWithValue("@Description", Description);
            cmd.Parameters.AddWithValue("@TechnicianID", TechnicianID);
            cmd.Parameters.AddWithValue("@NextMaintenanceDate", NextMaintenanceDate);
            return sql.ExecuteSqlWithID(cmd, "MainDB");
        }
        public void InsertIssueRecord(string AssetID, string LocationID, string IssueDate)
        {
            SqlCommand cmd = new SqlCommand(@"insert into tblAssetIssuedHistory (AssetID,LocationID,IssuedOn,IssuedBy)
                                            VALUES(@AssetID,@LocationID,@IssuedOn,@IssuedBy); update tblAsset set AssetStatus=2 where AssetID=@AssetID");
            cmd.Parameters.AddWithValue("@AssetID", AssetID);
            cmd.Parameters.AddWithValue("@LocationID", LocationID);
            cmd.Parameters.AddWithValue("@IssuedOn", IssueDate);
            cmd.Parameters.AddWithValue("@IssuedBy", ReadCookie.GetCookieValue("UserID"));
            sql.ExecuteSql(cmd, "MainDB");
        }
        public int ReleaseAsset(string AssetID)
        {
            try
            {
               SqlCommand cmd = new SqlCommand(@"update tblAsset set AssetStatus=1 where AssetID=@AssetID;
                                            update tblAssetIssuedHistory set ReleasedOn=GETDATE(),ReleasedBy=@ReleasedBy where AssetID=@AssetID");
                cmd.Parameters.AddWithValue("@AssetID", AssetID);
                cmd.Parameters.AddWithValue("@ReleasedBy", ReadCookie.GetCookieValue("UserID"));
                sql.ExecuteSql(cmd, "MainDB");
                return 1;
            }
            catch (Exception ex)
            {

                return 0;
            }

        }
        public int UploadSupplierOdooDocument(string SupplierID, string FileName)
        {
            try
            {
                SqlCommand cmd = new SqlCommand(@"insert into tblSupplierOdooDocuments (SupplierID,FileName,UploadedBy,UploadedOn)
                                                    values (@SupplierID,@FileName,@UploadedBy,GETDATE())");
                cmd.Parameters.AddWithValue("@SupplierID", SupplierID);
                cmd.Parameters.AddWithValue("@FileName", FileName);
                cmd.Parameters.AddWithValue("@UploadedBy", ReadCookie.GetCookieValue("UserID"));
                sql.ExecuteSql(cmd, "MainDB");
                return 1;
            }
            catch (Exception ex)
            {

                return 0;
            }
        }
        internal List<SupplierOdooDocuments> GetOdooDocuments(string SupplierID)
        {
            SqlCommand cmd = new SqlCommand(@"select d.Filename,CONVERT(varchar,UploadedOn,106) UploadedOn,u.Username UploadedBy
from tblSupplierOdooDocuments d 
INNER JOIN tblUsers u on u.UserID=d.UploadedBy
where d.SupplierID=@SupplierID order by d.ID desc");
            cmd.Parameters.AddWithValue("@SupplierID", SupplierID);
            DataSet ds = sql.GetTableCMD(cmd, "MainDB");
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].AsEnumerable().Select(c => new SupplierOdooDocuments
                {
                    FileName = c.Field<string>("Filename"),
                    UploadedBy = c.Field<string>("UploadedBy"),
                    UploadedOn = c.Field<string>("UploadedOn")
                }).ToList();
            else return null;
        }
    }
}