using PrompCorpFacilityManagementSystem.DBFunctions;
using PrompCorpFacilityManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PrompCorpFacilityManagementSystem.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        Login Login = new Login();
        public ActionResult Index()
        {
            return View("Login");
        }
        [HttpPost]
        public ActionResult LoginMeIn(string u, string p)
        {
            try
            {
                List<LoginModel> login = Login.LoginMe(u, p);
                if (login != null)
                {
                    HttpCookie userInfo = new HttpCookie("userInfo");
                    userInfo["UserID"] = login[0].UserID.ToString();
                    userInfo["UserName"] = login[0].Username;                    
                    userInfo["RoleName"] = login[0].Role;
                    

                    userInfo.Expires.Add(new TimeSpan(1, 0, 0));
                    Response.Cookies.Add(userInfo);
                    string RoleID = login[0].Role;
                    return Json(new { RoleID = RoleID });
                }
                else
                    return Content("Invalid Credentials");
            }
            catch (Exception ex)
            {

                return Content("error");
            }
        }
        public ActionResult Logout()
        {
            PrompCorpFacilityManagementSystem.DBFunctions.ReadCookie.RemoveCookie();
            return Content("1");
        }
    }
}