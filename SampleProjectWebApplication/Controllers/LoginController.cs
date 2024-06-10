using SampleProjectWebApplication.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SampleProjectWebApplication.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(Login lc)
        {
            string connectionString = "Data Source= LAPTOP-9U2484MG\\SQLEXPRESS; Initial Catalog= Internship ; Integrated Security=True";
            SqlConnection con = new SqlConnection(connectionString);
            string query = "Select User_Name,Password from dbo.LoginDetails where User_Name= @User_Name and Password= @Password";
            con.Open();
            SqlCommand sqlcon = new SqlCommand(query, con);
            sqlcon.Parameters.AddWithValue("@User_Name", lc.User_Name);
            sqlcon.Parameters.AddWithValue("@Password", lc.Password);
            SqlDataReader sdr = sqlcon.ExecuteReader();
            if (sdr.Read())
            {
                Session["User_Name"] = lc.User_Name.ToString();
                return RedirectToAction("Welcome");
            }
            else
            {
                ViewData["Message"] = "Invalid Credentials!";
            }
            con.Close();
            return View();
        }

        public ActionResult Welcome()
        {
            return View();
        }
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(UserRegisteration ur)
        {
            SqlConnection con = new SqlConnection("Data Source= LAPTOP-9U2484MG\\SQLEXPRESS; Initial Catalog= Internship ; Integrated Security=True");
            string query = "INSERT LoginDetails(First_Name,Last_Name,Dob,User_Name,Password) VALUES(@First_Name,@Last_Name, @Dob,@User_Name,@Password)";
            query += " SELECT SCOPE_IDENTITY()";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                con.Open();
                cmd.Parameters.AddWithValue("@First_Name", ur.First_Name);
                cmd.Parameters.AddWithValue("@Last_Name", ur.Last_Name);
                cmd.Parameters.AddWithValue("@Dob", ur.Dob);
                cmd.Parameters.AddWithValue("@User_Name", ur.User_Name);
                cmd.Parameters.AddWithValue("@Password", ur.Password);
                cmd.ExecuteNonQuery();
                con.Close();
            }
            return RedirectToAction("Index","Login");
        }
    }
}