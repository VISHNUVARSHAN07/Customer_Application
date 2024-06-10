using SampleProjectWebApplication.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SampleProjectWebApplication.Controllers
{
    public class CustomerController : Controller
    {

        private readonly string connectionString = "Data Source= LAPTOP-9U2484MG\\SQLEXPRESS; Initial Catalog= Internship ; Integrated Security=True"; 
        // GET: Customer
        public ActionResult Index(string filter)
        {
            List<CustomerDetails> list = new List<CustomerDetails>();
            SqlConnection con = new SqlConnection(connectionString);
            try
            {
                if (string.IsNullOrEmpty(filter))
                {
                    string query = "Select m.Customer_Id,m.Name,Format(Cast(m.Valid_From as Date),'dd-MMM-yyyy') as Valid_From, Format(Cast(m.Valid_To as Date),'dd-MMM-yyyy') as Valid_To,m.Customer_Type,m.Customer_Status from Customer m";
                    con.Open();
                    SqlCommand cmd = new SqlCommand(query, con);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CustomerDetails customer = new CustomerDetails
                            {
                                Customer_Id = (int)reader["Customer_Id"],
                                Name = reader["Name"].ToString(),
                                Valid_From = Convert.ToDateTime(reader["Valid_From"]),
                                Valid_To = Convert.ToDateTime(reader["Valid_To"]),
                                Customer_Type = reader["Customer_Type"].ToString(),
                                Customer_Status = reader["Customer_Status"].ToString()
                            };
                            list.Add(customer);
                        }
                    }
                }
                else
                {
                    string query = "Select m.Customer_Id,m.Name,Format(Cast(m.Valid_From as Date),'dd-MMM-yyyy') as Valid_From, Format(Cast(m.Valid_To as Date),'dd-MMM-yyyy') as Valid_To,m.Customer_Type,m.Customer_Status from Customer m where Name like  " + "'" + filter + "%'";
                    con.Open();
                    SqlCommand cmd = new SqlCommand(query, con);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CustomerDetails customer = new CustomerDetails
                            {
                                Customer_Id = (int)reader["Customer_Id"],
                                Name = reader["Name"].ToString(),
                                Valid_From = Convert.ToDateTime(reader["Valid_From"]),
                                Valid_To = Convert.ToDateTime(reader["Valid_To"]),
                                Customer_Type = reader["Customer_Type"].ToString(),
                                Customer_Status = reader["Customer_Status"].ToString()
                            };
                            list.Add(customer);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
            return View("Index", list);
        }
        public ActionResult Create()
        {
            return View(new CustomerDetails());
        }

        public ActionResult Delete(int CustomerId)
        {
            SqlConnection con = new SqlConnection(connectionString);
            try
            {
                string query = "Delete From Customer_PersonalDetails Where Customer_Id=@Customer_Id";
                query += " SELECT SCOPE_IDENTITY()";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    cmd.Parameters.AddWithValue("@Customer_Id", CustomerId);
                    cmd.ExecuteNonQuery();
                    con.Close();
                }

                string query1 = "Delete From Customer Where Customer_Id=@Customer_Id";
                query1 += " SELECT SCOPE_IDENTITY()";
                using (SqlCommand cmd = new SqlCommand(query1, con))
                {
                    con.Open();
                    cmd.Parameters.AddWithValue("@Customer_Id", CustomerId);
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
            return RedirectToAction("Index");
        }
        public ActionResult Edit(int CustomerId)
        {
            List<CustomerDetails> list = new List<CustomerDetails>();
            CustomerDetails instance = new CustomerDetails();
            SqlConnection con = new SqlConnection(connectionString);
            try
            {
                con.Open();
                string query = "Select * from Customer Where Customer_Id =" + CustomerId;
                SqlCommand cmd = new SqlCommand(query, con);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        instance.Customer_Id = (int)reader["Customer_Id"];
                        instance.Name = reader["Name"].ToString();
                        instance.Valid_From = Convert.ToDateTime(reader["Valid_From"].ToString());
                        instance.Valid_To = Convert.ToDateTime(reader["Valid_To"].ToString());
                        instance.Customer_Type = reader["Customer_Type"].ToString();
                        instance.Customer_Status = reader["Customer_Status"].ToString();
                        instance.Phone_Number = reader["Phone_Number"].ToString();
                        list.Add(instance);
                    }
                }
                string query2 = "Select * from Customer_PersonalDetails Where Customer_Id=" + CustomerId;
                SqlCommand cmd2 = new SqlCommand(query2, con);
                using (SqlDataReader reader = cmd2.ExecuteReader())
                {
                    var index = 0;
                    while (reader.Read())
                    {
                        Personal_Details personal_Details = new Personal_Details();
                        personal_Details.Customer_Id = (int)reader["Customer_Id"];
                        personal_Details.Personal_Id = (int)reader["Personal_Id"];
                        personal_Details.Address_Type = reader["Address_Type"].ToString();
                        personal_Details.Street = reader["Street"].ToString();
                        personal_Details.Area = reader["Area"].ToString();
                        personal_Details.Location = reader["Location"].ToString();
                        personal_Details.Pincode = reader["Pincode"].ToString();
                        personal_Details.IsDeleted = false;
                        personal_Details.RowIndex = index;
                        list[0].Personal_Details.Add(personal_Details);
                        index++;
                    }
                }
                instance = list.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
            return View("Create", instance);
        }

        [HttpPost]
        public JsonResult SaveAndUpdate(CustomerDetails instance)
        {
            SqlConnection con = new SqlConnection(connectionString);
            try
            {
                if (instance.Customer_Id == 0)
                {
                    string query = "Insert into Customer (Name,Valid_From,Valid_To,Customer_Type,Customer_Status,Phone_Number) Values(@Name,@Valid_From,@Valid_To,@Customer_Type,@Customer_Status,@Phone_Number)";
                    query += " SELECT SCOPE_IDENTITY()";
                    SqlCommand cmd = new SqlCommand(query, con);
                    con.Open();
                    cmd.Parameters.AddWithValue("@Name", instance.Name);
                    cmd.Parameters.AddWithValue("@Valid_From", instance.Valid_From);
                    cmd.Parameters.AddWithValue("@Valid_To", instance.Valid_To);
                    cmd.Parameters.AddWithValue("@Customer_Type", instance.Customer_Type);
                    cmd.Parameters.AddWithValue("@Customer_Status", instance.Customer_Status);
                    cmd.Parameters.AddWithValue("@Phone_Number", instance.Phone_Number);
                    var output = cmd.ExecuteScalar();

                    for (var i = 0; i <= instance.Personal_Details.Count - 1; i++)
                    {
                        string query2 = "INSERT into Customer_PersonalDetails(Customer_Id,Address_Type,Street,Area,Location,Pincode) Values(@Customer_Id,@Address_Type,@Street,@Area,@Location,@Pincode)";
                        SqlCommand cmd2 = new SqlCommand(query2, con);
                        cmd2.Parameters.AddWithValue("@Customer_Id", output);
                        cmd2.Parameters.AddWithValue("@Address_Type", instance.Personal_Details[i].Address_Type);
                        cmd2.Parameters.AddWithValue("@Street", instance.Personal_Details[i].Street);
                        cmd2.Parameters.AddWithValue("@Area", instance.Personal_Details[i].Area);
                        cmd2.Parameters.AddWithValue("@Location", instance.Personal_Details[i].Location);
                        cmd2.Parameters.AddWithValue("@Pincode", instance.Personal_Details[i].Pincode);
                        cmd2.ExecuteNonQuery();
                    }
                }
                else
                {
                    string query = "Update Customer" + " Set Name=@Name, Valid_From=@Valid_From, Valid_To=@Valid_To, Customer_Type=@Customer_Type, Customer_Status=@Customer_Status, Phone_Number=@Phone_Number " + " Where Customer_Id=@Customer_Id ";
                    SqlCommand cmd = new SqlCommand(query, con);
                    con.Open();
                    cmd.Parameters.AddWithValue("@Customer_Id", instance.Customer_Id);
                    cmd.Parameters.AddWithValue("@Name", instance.Name);
                    cmd.Parameters.AddWithValue("@Valid_From", instance.Valid_From);
                    cmd.Parameters.AddWithValue("@Valid_To", instance.Valid_To);
                    cmd.Parameters.AddWithValue("@Customer_Type", instance.Customer_Type);
                    cmd.Parameters.AddWithValue("@Customer_Status", instance.Customer_Status);
                    cmd.Parameters.AddWithValue("@Phone_Number", instance.Phone_Number);
                    cmd.ExecuteNonQuery();
                    for (var i = 0; i <= instance.Personal_Details.Count - 1; i++)
                    {
                        if (instance.Personal_Details[i].Personal_Id != 0 || instance.Personal_Details[i].IsDeleted)
                        {
                            if (instance.Personal_Details[i].IsDeleted == false)
                            {
                                string query2 = "Update Customer_PersonalDetails" + " SET Customer_Id=@Customer_Id,Address_Type=@Address_Type,Street=@Street,Area=@Area, Location=@Location, Pincode=@Pincode" + " Where Personal_Id=@Personal_Id";
                                SqlCommand cmd2 = new SqlCommand(query2, con);
                                cmd2.Parameters.AddWithValue("@Personal_Id", instance.Personal_Details[i].Personal_Id);
                                cmd2.Parameters.AddWithValue("@Customer_Id", instance.Customer_Id);
                                cmd2.Parameters.AddWithValue("@Address_Type", instance.Personal_Details[i].Address_Type);
                                cmd2.Parameters.AddWithValue("@Street", instance.Personal_Details[i].Street);
                                cmd2.Parameters.AddWithValue("@Area", instance.Personal_Details[i].Area);
                                cmd2.Parameters.AddWithValue("@Location", instance.Personal_Details[i].Location);
                                cmd2.Parameters.AddWithValue("@Pincode", instance.Personal_Details[i].Pincode);
                                cmd2.ExecuteNonQuery();
                            }
                            else
                            {
                                string query3 = "Delete From Customer_PersonalDetails Where Personal_Id=@Personal_Id";
                                query3 += " SELECT SCOPE_IDENTITY()";
                                using (SqlCommand cmd2 = new SqlCommand(query3, con))
                                {
                                    cmd2.Parameters.AddWithValue("@Personal_Id", instance.Personal_Details[i].Personal_Id);
                                    cmd2.ExecuteNonQuery();
                                }
                            }
                        }
                        else
                        {
                            string query2 = "INSERT into Customer_PersonalDetails(Customer_Id,Address_Type,Street,Area,Location,Pincode) Values(@Customer_Id,@Address_Type,@Street,@Area,@Location,@Pincode)";
                            SqlCommand cmd2 = new SqlCommand(query2, con);
                            cmd2.Parameters.AddWithValue("@Customer_Id", instance.Customer_Id);
                            cmd2.Parameters.AddWithValue("@Address_Type", instance.Personal_Details[i].Address_Type);
                            cmd2.Parameters.AddWithValue("@Street", instance.Personal_Details[i].Street);
                            cmd2.Parameters.AddWithValue("@Area", instance.Personal_Details[i].Area);
                            cmd2.Parameters.AddWithValue("@Location", instance.Personal_Details[i].Location);
                            cmd2.Parameters.AddWithValue("@Pincode", instance.Personal_Details[i].Pincode);
                            cmd2.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
            return Json("Saved Successfully");
        }
    }
}