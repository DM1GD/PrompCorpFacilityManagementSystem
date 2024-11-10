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
    public class Login
    {
        Sql sql = new Sql();
        LoginModel login = new LoginModel();
        internal List<LoginModel> LoginMe(string Username, string userPassword)
        {
            SqlCommand cmd = new SqlCommand(@"select UserID,Username,UserPassword,Role from tblUsers
                where Username=@Username and UserPassword=@UserPassword and Status=1");
            cmd.Parameters.AddWithValue("@Username", Username);
            cmd.Parameters.AddWithValue("@UserPassword", userPassword);
            DataSet ds = sql.GetTableCMD(cmd, "MainDB");
            if (ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0].AsEnumerable().Select(s => new LoginModel
                {
                    UserID = s.Field<int>("UserID"),
                    Username = s.Field<string>("Username"),                    
                    Role = s.Field<string>("Role")
                }).ToList();
            }
            else
                return null;

        }
    }
}