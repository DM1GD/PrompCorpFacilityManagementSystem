using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Web.Mvc;

namespace PrompCorpFacilityManagementSystem.DBFunctions
{
    public class Sql
    {
        public SqlConnection con;
        SqlDataReader dr;
        public DataSet GetTableCMD(SqlCommand cmd, string db)
        {
            DataSet ds = new DataSet();
            OpenConnection(db);
            cmd.Connection = con;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds, "tablename");
            con.Close();
            return ds;
        }
        public void OpenConnection(string db)
        {
            string connectionString = ConfigurationManager.AppSettings[db].ToString();
            try
            {
                con = new SqlConnection();
                con.ConnectionString = connectionString;
                con.Open();
            }
            catch (SqlException se)
            {
                string mesg = se.Message;

            }
        }
        public SqlDataReader GetDR(SqlCommand cmd, string db)
        {
            OpenConnection(db);
            cmd.Connection = con;
            dr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            return dr;
        }
        public int ExecuteSqlWithID(SqlCommand cmd, string db)
        {
            int ID = 0;
            OpenConnection(db);
            cmd.Connection = con;
            ID = int.Parse(cmd.ExecuteScalar().ToString());
            cmd.Dispose();
            con.Close();
            return ID;
        }
        public void ExecuteSql(SqlCommand cmd, string db)
        {
            OpenConnection(db);
            cmd.Connection = con;
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            con.Close();
        }
    }

    public class SessionCheckFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.HttpContext.Request.IsAjaxRequest())
            {
                if (!HttpContext.Current.Request.Url.ToString().ToLower().Contains("/login"))
                    if (ReadCookie.GetCookieValue("UserID") == null)
                        HttpContext.Current.Response.Redirect("/login/");
                   
            }
        }
    }

    public static class ReadCookie
    {
        public static string GetCookieValue(string key)
        {
            HttpCookie reqCookies = HttpContext.Current.Request.Cookies["userInfo"];
            if (reqCookies != null && reqCookies[key] != null)
                return reqCookies[key].ToString();
            else return "";
        }
        public static void RemoveCookie()
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies["userInfo"];
            if (cookie != null)
            {
                string[] subkeys = cookie.Values.AllKeys;
                foreach (string value in subkeys)
                {
                    cookie.Values.Remove(value);
                }
                cookie.Expires = DateTime.Now;
                HttpContext.Current.Response.Cookies.Add(cookie);
                HttpContext.Current.Session.Abandon();
            }
        }
    }
}