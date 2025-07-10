using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public DepartmentsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                select DepartmentID,DepartmentName 
                from dbo.Departments
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
        public JsonResult Post(Departments dep)
        {
            string query = @"

                insert into dbo.Departments(DepartmentName) 
                values (@DepartmentName);
            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReder;
            using (SqlConnection mycon = new SqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (SqlCommand mycommand = new SqlCommand(query, mycon))
                {
                    mycommand.Parameters.AddWithValue("@DepartmentName", dep.DepartmentName);
                    myReder = mycommand.ExecuteReader();
                    table.Load(myReder);
                    myReder.Close();
                    mycon.Close();

                }
            }

            return new JsonResult("Added sucessfully");
        }
        [HttpPut]
        public JsonResult Update(Departments dep)
        {
            string query = @"
               update dbo.Departments  set
               DepartmentName =@DepartmentName
               where DepartmentID =@DepartmentID;
            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReder;
            using (SqlConnection mycon = new SqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (SqlCommand mycommand = new SqlCommand(query, mycon))
                {
                    mycommand.Parameters.AddWithValue("@DepartmentID", dep.DepartmentID);
                    mycommand.Parameters.AddWithValue("@DepartmentName", dep.DepartmentName);

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
               delete from dbo.Departments 
               where DepartmentID =@DepartmentID;
            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReder;
            using (SqlConnection mycon = new SqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (SqlCommand mycommand = new SqlCommand(query, mycon))
                {
                    mycommand.Parameters.AddWithValue("@DepartmentID", id);
                    
                    myReder = mycommand.ExecuteReader();
                    table.Load(myReder);
                    myReder.Close();
                    mycon.Close();

                }
            }


            return new JsonResult("Deleted sucessfully");
        }
    }
       


}

