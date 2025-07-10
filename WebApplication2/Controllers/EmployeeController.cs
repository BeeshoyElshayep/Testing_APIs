using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        public EmployeeController(IConfiguration configuration,IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }
        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                select EmployeeID,EmployeeName, Department,DateOfjoinIng,PhotoFileName  
                from dbo.Employee
            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReder;
            using (SqlConnection mycon = new SqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (SqlCommand mycommand = new SqlCommand(query, mycon))
                {
                    myReder = mycommand.ExecuteReader();
                    table.Load(myReder);
                    myReder.Close();
                    mycon.Close();

                }
            }

            return new JsonResult(table);
        }
        [HttpPost]
        public JsonResult Post(Emloyee emp)
        {
            string query = @"
                insert into dbo.Employee(EmployeeName,Department,DateOfjoining,PhotoFileName) 
                values                  (@EmployeeName,@Department,@DateOfjoining,@PhotoFileName);
            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReder;
            using (SqlConnection mycon = new SqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (SqlCommand mycommand = new SqlCommand(query, mycon))
                {
                    mycommand.Parameters.AddWithValue("@EmployeeName", emp.EmployeeName);
                    mycommand.Parameters.AddWithValue("@Department", emp.Department);
                    mycommand.Parameters.AddWithValue("@DateOfjoining", emp.DateOfJoining);
                    mycommand.Parameters.AddWithValue("@PhotoFileName", emp.PhotoFileName);
                    myReder = mycommand.ExecuteReader();
                    table.Load(myReder);
                    myReder.Close();
                    mycon.Close();

                }
            }

            return new JsonResult("Added sucessfully");
        }
        [HttpPut]
        public JsonResult Update(Emloyee emp)
        {
            string query = @"
               update dbo.Employee  set
               EmployeeName =@EmployeeName,
               Department  =@Department,
               DateOfjoining =@DateOfjoining,
               PhotoFileName =@PhotoFileName
               where EmployeeID =@EmployeeID;
            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReder;
            using (SqlConnection mycon = new SqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (SqlCommand mycommand = new SqlCommand(query, mycon))
                {
                    mycommand.Parameters.AddWithValue("@EmployeeID", emp.EmlpoyeeID);
                    mycommand.Parameters.AddWithValue("@EmployeeName", emp.EmployeeName);
                    mycommand.Parameters.AddWithValue("@Department", emp.Department);
                    mycommand.Parameters.AddWithValue("@DateOfjoining", emp.DateOfJoining);
                    mycommand.Parameters.AddWithValue("@PhotoFileName", emp.PhotoFileName);

                    myReder = mycommand.ExecuteReader();
                    table.Load(myReder);
                    myReder.Close();
                    mycon.Close();

                }
            }


            return new JsonResult("Updated sucessfully");
        }
        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"
               delete from dbo.Employee 
               where EmployeeID =@EmployeeID;
            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReder;
            using (SqlConnection mycon = new SqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (SqlCommand mycommand = new SqlCommand(query, mycon))
                {
                    mycommand.Parameters.AddWithValue("@EmployeeID", id);

                    myReder = mycommand.ExecuteReader();
                    table.Load(myReder);
                    myReder.Close();
                    mycon.Close();

                }
            }


            return new JsonResult("Deleted sucessfully");
        }

        [Route("SaveFile")]

        [HttpPost]
        public JsonResult SaveFile()
        {
            try
            {
                var httpRequet = Request.Form;
                var postedFile = httpRequet.Files[0];
                string filename=postedFile.FileName;
                var phsicalPath = _env.ContentRootPath + "/Photos/" + filename;
                using(var stream=new FileStream(phsicalPath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }
                return new JsonResult(filename);
            }
            catch(Exception)
            {
                return new JsonResult("anonymous.png");
            }
        }
    }
}
