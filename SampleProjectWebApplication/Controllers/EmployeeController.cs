using SampleProjectWebApplication.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SampleProjectWebApplication.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly string connectionString = "Data Source= LAPTOP-9U2484MG\\SQLEXPRESS; Initial Catalog= Internship ; Integrated Security=True";

        // GET: Employee
        public ActionResult Index()
        {
            List<EmployeeDetails> list = new List<EmployeeDetails>();
            SqlConnection con = new SqlConnection(connectionString);
            try
            {
                string query = "Select * from MiniProject";
                con.Open();
                SqlCommand cmd = new SqlCommand(query, con);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        EmployeeDetails Employee = new EmployeeDetails();
                        Employee.Emp_Id = (int)reader["Emp_Id"];
                        Employee.Employee_Number = (int)reader["Employee_Number"];
                        Employee.Employee_Name = reader["Employee_Name"].ToString();
                        Employee.Department = reader["Department"].ToString();
                        Employee.Status = reader["Status"].ToString();
                        list.Add(Employee);
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
            return View(new EmployeeDetails());
        }

        public ActionResult Delete(int EmpId)
        {
            SqlConnection con = new SqlConnection(connectionString);
            try
            {
                string query = "Delete From MiniProjectAsset Where Emp_Id=@Emp_Id";
                query += " SELECT SCOPE_IDENTITY()";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    cmd.Parameters.AddWithValue("@Emp_Id", EmpId);
                    cmd.ExecuteNonQuery();
                    con.Close();
                }

                string query1 = "Delete From Miniprojectsalary Where Emp_Id=@Emp_Id";
                query1 += " SELECT SCOPE_IDENTITY()";
                using (SqlCommand cmd = new SqlCommand(query1, con))
                {
                    con.Open();
                    cmd.Parameters.AddWithValue("@Emp_Id", EmpId);
                    cmd.ExecuteNonQuery();
                    con.Close();
                }

                string query2 = "Delete From Miniproject Where Emp_Id=@Emp_Id";
                query2 += " SELECT SCOPE_IDENTITY()";
                using (SqlCommand cmd1 = new SqlCommand(query2, con))
                {
                    con.Open();
                    cmd1.Parameters.AddWithValue("@Emp_Id", EmpId);
                    cmd1.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
                //ViewBag.Message = "Are You Sure Want To Delete?";
            }

            return RedirectToAction("Index");
        }
        public ActionResult Edit(int EmpId)
        {
            List<EmployeeDetails> list = new List<EmployeeDetails>();
            EmployeeDetails instance = new EmployeeDetails();
            SqlConnection con = new SqlConnection(connectionString);
            try
            {
                con.Open();
                string query = "Select * from MiniProject Where Emp_Id=" + EmpId;
                SqlCommand cmd = new SqlCommand(query, con);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        instance.Emp_Id = (int)reader["Emp_Id"];
                        instance.Employee_Number = (int)reader["Employee_Number"];
                        instance.Employee_Name = reader["Employee_Name"].ToString();
                        instance.Department = reader["Department"].ToString();
                        instance.Gender = reader["Gender"].ToString();
                        instance.Joining_Date = Convert.ToDateTime(reader["Joining_Date"].ToString());
                        instance.Status = reader["Status"].ToString();
                        list.Add(instance);
                    }
                }

                string query1 = "Select * from MiniProjectSalary Where Emp_Id=" + EmpId;
                SqlCommand cmd1 = new SqlCommand(query1, con);
                using (SqlDataReader reader = cmd1.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list[0].Monthly_Basic_Salary = reader["Monthly_Basic_Salary"].ToString();
                        list[0].Monthly_HRA = reader["Monthly_HRA"].ToString();
                        list[0].Monthly_TA = reader["Monthly_TA"].ToString();
                        list[0].Monthly_PF = reader["Monthly_PF"].ToString();
                        list[0].Monthly_ESI = reader["Monthly_ESI"].ToString();
                        list[0].Monthly_Gross = reader["Monthly_Gross"].ToString();
                        list[0].Monthly_Takehome = reader["Monthly_Takehome"].ToString();
                        list[0].Yearly_Gross = reader["Yearly_Gross"].ToString();
                        list[0].Yearly_Takehome = reader["Yearly_Takehome"].ToString();
                    }
                }

                string query2 = "Select * from MiniprojectAsset Where Emp_Id=" + EmpId;
                SqlCommand cmd2 = new SqlCommand(query2, con);
                using (SqlDataReader reader = cmd2.ExecuteReader())
                {
                    var index = 0;
                    while (reader.Read())
                    {
                        AssetDetail assetDetail = new AssetDetail();
                        assetDetail.Emp_Id = (int)reader["Emp_Id"];
                        assetDetail.Asset_Id = (int)reader["Asset_Id"];
                        assetDetail.Asset_Type = reader["Asset_Type"].ToString();
                        assetDetail.Description = reader["Description"].ToString();
                        assetDetail.Brand_Name = reader["Brand_Name"].ToString();
                        assetDetail.Model_Name = reader["Model_Name"].ToString();
                        assetDetail.Assigned_To = reader["Assigned_To"].ToString();
                        assetDetail.IsDeleted = false;
                        assetDetail.RowIndex = index;
                        list[0].AssetDetails.Add(assetDetail);
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
        public JsonResult SaveAndUpdate(EmployeeDetails instance)
        {
            SqlConnection con = new SqlConnection(connectionString);
            try
            {
                if (instance.Emp_Id == 0)
                {
                    string query = "INSERT into MiniProject(Employee_Number,Employee_Name,Department,Status,Joining_Date,Gender) VALUES(@Employee_Number,@Employee_Name, @Department,@Status,@Joining_Date,@Gender)";
                    query += " SELECT SCOPE_IDENTITY()";

                    SqlCommand cmd = new SqlCommand(query, con);
                    con.Open();
                    cmd.Parameters.AddWithValue("@Employee_Number", instance.Employee_Number);
                    cmd.Parameters.AddWithValue("@Employee_Name", instance.Employee_Name);
                    cmd.Parameters.AddWithValue("@Department", instance.Department);
                    cmd.Parameters.AddWithValue("@Status", instance.Status);
                    cmd.Parameters.AddWithValue("@Joining_Date", instance.Joining_Date);
                    cmd.Parameters.AddWithValue("@Gender", instance.Gender);
                    var output = cmd.ExecuteScalar();


                    string query1 = "INSERT into MiniProjectSalary(Emp_Id,Monthly_Basic_Salary,Monthly_HRA,Monthly_TA,Monthly_PF,Monthly_ESI,Monthly_Gross,Monthly_Takehome,Yearly_Gross,Yearly_Takehome) Values(@Emp_id,@Monthly_Basic_Salary,@Monthly_HRA,@Monthly_TA,@Monthly_PF,@Monthly_ESI,@Monthly_Gross,@Monthly_Takehome,@Yearly_Gross,@Yearly_Takehome)";
                    SqlCommand cmd1 = new SqlCommand(query1, con);
                    cmd1.Parameters.AddWithValue("@Emp_Id", output);
                    cmd1.Parameters.AddWithValue("@Monthly_Basic_Salary", instance.Monthly_Basic_Salary);
                    cmd1.Parameters.AddWithValue("@Monthly_HRA", instance.Monthly_HRA);
                    cmd1.Parameters.AddWithValue("@Monthly_TA", instance.Monthly_TA);
                    cmd1.Parameters.AddWithValue("@Monthly_PF", instance.Monthly_PF);
                    cmd1.Parameters.AddWithValue("@Monthly_ESI", instance.Monthly_ESI);
                    cmd1.Parameters.AddWithValue("@Monthly_Gross", instance.Monthly_Gross);
                    cmd1.Parameters.AddWithValue("@Monthly_Takehome", instance.Monthly_Takehome);
                    cmd1.Parameters.AddWithValue("@Yearly_Gross", instance.Yearly_Gross);
                    cmd1.Parameters.AddWithValue("@Yearly_Takehome", instance.Yearly_Takehome);
                    cmd1.ExecuteNonQuery();

                    for (var i = 0; i <= instance.AssetDetails.Count - 1; i++)
                    {
                        string query2 = "INSERT into MiniProjectAsset(Emp_Id,Asset_Type,Description,Brand_Name,Model_Name,Assigned_To) Values(@Emp_id,@Asset_Type,@Description,@Brand_Name,@Model_Name,@Assigned_To)";
                        SqlCommand cmd2 = new SqlCommand(query2, con);
                        cmd2.Parameters.AddWithValue("@Emp_Id", output);
                        cmd2.Parameters.AddWithValue("@Asset_Type", instance.AssetDetails[i].Asset_Type);
                        cmd2.Parameters.AddWithValue("@Description", instance.AssetDetails[i].Description);
                        cmd2.Parameters.AddWithValue("@Brand_Name", instance.AssetDetails[i].Brand_Name);
                        cmd2.Parameters.AddWithValue("@Model_Name", instance.AssetDetails[i].Model_Name);
                        cmd2.Parameters.AddWithValue("@Assigned_To", instance.AssetDetails[i].Assigned_To);
                        cmd2.ExecuteNonQuery();
                    }
                }
                else
                {
                    string query = "Update Miniproject" + " SET Employee_Number=@Employee_Number, Employee_Name=@Employee_Name, Department=@Department, Status=@Status, Joining_Date=@Joining_Date, Gender=@Gender" + " Where Emp_Id=@Emp_Id";
                    SqlCommand cmd = new SqlCommand(query, con);
                    con.Open();
                    cmd.Parameters.AddWithValue("@Emp_Id", instance.Emp_Id);
                    cmd.Parameters.AddWithValue("@Employee_Number", instance.Employee_Number);
                    cmd.Parameters.AddWithValue("@Employee_Name", instance.Employee_Name);
                    cmd.Parameters.AddWithValue("@Department", instance.Department);
                    cmd.Parameters.AddWithValue("@Status", instance.Status);
                    cmd.Parameters.AddWithValue("@Joining_Date", instance.Joining_Date);
                    cmd.Parameters.AddWithValue("@Gender", instance.Gender);
                    cmd.ExecuteNonQuery();


                    string query1 = "Update MiniProjectSalary" + " SET Emp_Id=@Emp_Id,Monthly_Basic_Salary=@Monthly_Basic_Salary, Monthly_HRA=@Monthly_HRA,Monthly_TA=@Monthly_TA,Monthly_PF=@Monthly_PF,Monthly_ESI=@Monthly_ESI,Monthly_Gross=@Monthly_Gross,Monthly_Takehome=@Monthly_Takehome,Yearly_Gross=@Yearly_Gross,Yearly_Takehome=@Yearly_Takehome" + " Where Emp_Id=@Emp_Id";
                    SqlCommand cmd1 = new SqlCommand(query1, con);
                    cmd1.Parameters.AddWithValue("@Emp_Id", instance.Emp_Id);
                    cmd1.Parameters.AddWithValue("@Monthly_Basic_Salary", instance.Monthly_Basic_Salary);
                    cmd1.Parameters.AddWithValue("@Monthly_HRA", instance.Monthly_HRA);
                    cmd1.Parameters.AddWithValue("@Monthly_TA", instance.Monthly_TA);
                    cmd1.Parameters.AddWithValue("@Monthly_PF", instance.Monthly_PF);
                    cmd1.Parameters.AddWithValue("@Monthly_ESI", instance.Monthly_ESI);
                    cmd1.Parameters.AddWithValue("@Monthly_Gross", instance.Monthly_Gross);
                    cmd1.Parameters.AddWithValue("@Monthly_Takehome", instance.Monthly_Takehome);
                    cmd1.Parameters.AddWithValue("@Yearly_Gross", instance.Yearly_Gross);
                    cmd1.Parameters.AddWithValue("@Yearly_Takehome", instance.Yearly_Takehome);
                    cmd1.ExecuteNonQuery();

                    for (var i = 0; i <= instance.AssetDetails.Count - 1; i++)
                    {
                        if (instance.AssetDetails[i].Asset_Id != 0 || instance.AssetDetails[i].IsDeleted)
                        {
                            if (instance.AssetDetails[i].IsDeleted == false)
                            {
                                //string query2 = "Update MiniprojectAsset" + " SET Emp_Id=@Emp_Id,Asset_Id=@Asset_Id,Asset_Type=@Asset_Type,Description=@Description,Brand_Name=@Brand_Name, Model_Name=@Model_Name, Assigned_To=@Assigned_To" + " Where Emp_Id=@Emp_Id";
                                //string query2 = "Update MiniprojectAsset" + " SET Emp_Id=@Emp_Id,Asset_Id=@Asset_Id,Asset_Type=@Asset_Type,Description=@Description,Brand_Name=@Brand_Name, Model_Name=@Model_Name, Assigned_To=@Assigned_To" + " Where Emp_Id=@Emp_Id";
                                string query2 = "Update MiniprojectAsset" + " SET Emp_Id=@Emp_Id,Asset_Type=@Asset_Type,Description=@Description,Brand_Name=@Brand_Name, Model_Name=@Model_Name, Assigned_To=@Assigned_To" + " Where Asset_Id=@Asset_Id";
                                SqlCommand cmd2 = new SqlCommand(query2, con);
                                cmd2.Parameters.AddWithValue("@Asset_Id", instance.AssetDetails[i].Asset_Id);
                                cmd2.Parameters.AddWithValue("@Emp_Id", instance.Emp_Id);
                                cmd2.Parameters.AddWithValue("@Asset_Type", instance.AssetDetails[i].Asset_Type);
                                cmd2.Parameters.AddWithValue("@Description", instance.AssetDetails[i].Description);
                                cmd2.Parameters.AddWithValue("@Brand_Name", instance.AssetDetails[i].Brand_Name);
                                cmd2.Parameters.AddWithValue("@Model_Name", instance.AssetDetails[i].Model_Name);
                                cmd2.Parameters.AddWithValue("@Assigned_To", instance.AssetDetails[i].Assigned_To);
                                cmd2.ExecuteNonQuery();
                            }
                            else
                            {
                                string query3 = "Delete From MiniProjectAsset Where Asset_Id=@Asset_Id";
                                query3 += " SELECT SCOPE_IDENTITY()";
                                using (SqlCommand cmd2 = new SqlCommand(query3, con))
                                {
                                    cmd2.Parameters.AddWithValue("@Asset_Id", instance.AssetDetails[i].Asset_Id);
                                    cmd2.ExecuteNonQuery();
                                }
                            }
                        }
                        else
                        {
                            string query2 = "INSERT into MiniProjectAsset(Emp_Id,Asset_Type,Description,Brand_Name,Model_Name,Assigned_To) Values(@Emp_id,@Asset_Type,@Description,@Brand_Name,@Model_Name,@Assigned_To)";
                            SqlCommand cmd2 = new SqlCommand(query2, con);
                            cmd2.Parameters.AddWithValue("@Emp_Id", instance.Emp_Id);
                            cmd2.Parameters.AddWithValue("@Asset_Type", instance.AssetDetails[i].Asset_Type);
                            cmd2.Parameters.AddWithValue("@Description", instance.AssetDetails[i].Description);
                            cmd2.Parameters.AddWithValue("@Brand_Name", instance.AssetDetails[i].Brand_Name);
                            cmd2.Parameters.AddWithValue("@Model_Name", instance.AssetDetails[i].Model_Name);
                            cmd2.Parameters.AddWithValue("@Assigned_To", instance.AssetDetails[i].Assigned_To);
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