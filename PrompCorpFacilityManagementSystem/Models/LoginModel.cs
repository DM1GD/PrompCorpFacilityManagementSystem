using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrompCorpFacilityManagementSystem.Models
{
    public class LoginModel
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string UserPassword { get; set; }
        public string Role { get; set; }
        public int Status { get; set; }
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