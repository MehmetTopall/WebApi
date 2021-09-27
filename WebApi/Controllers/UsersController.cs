using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public UsersController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
               select UserId as ""UserId"",
                        FullName as ""FullName"",
                          UserName as ""UserName"",
                            Phone as ""Phone"",
                              Email as ""Email"",
                                Address as ""Address""
               from Users
            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("UserAppCon");
            NpgsqlDataReader myReader;
            using (NpgsqlConnection myCon=new NpgsqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (NpgsqlCommand myCommand=new NpgsqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult(table);
        }

        [HttpPost]
        public JsonResult Post(Users users)
        {
            string query = @"
               insert into Users(FullName,UserName,Phone,Email,Address) 
                            values(@FullName,@UserName,@Phone,@Email,@Address)";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("UserAppCon");
            NpgsqlDataReader myReader;
            using (NpgsqlConnection myCon = new NpgsqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@FullName", users.FullName);
                    myCommand.Parameters.AddWithValue("@UserName", users.UserName);
                    myCommand.Parameters.AddWithValue("@Phone", users.Phone);
                    myCommand.Parameters.AddWithValue("@Email", users.Email);
                    myCommand.Parameters.AddWithValue("@Address", users.Address);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Ekleme Başarılı!");
        }

        [HttpPut]
        public JsonResult Put(Users users)
        {
            string query = @"
               update Users
                         set FullName=@FullName,
                                 UserName=@UserName,
                                   Phone=@Phone,
                                     Email=@Email,
                                       Address=@Address
                           where UserId=@UserId;
";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("UserAppCon");
            NpgsqlDataReader myReader;
            using (NpgsqlConnection myCon = new NpgsqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@UserId",users.UserId);
                    myCommand.Parameters.AddWithValue("@FullName", users.FullName);
                    myCommand.Parameters.AddWithValue("@UserName", users.UserName);
                    myCommand.Parameters.AddWithValue("@Phone", users.Phone);
                    myCommand.Parameters.AddWithValue("@Email", users.Email);
                    myCommand.Parameters.AddWithValue("@Address", users.Address);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Güncelleme Başarılı!");
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"
               delete from Users 
                           where UserId=@UserId";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("UserAppCon");
            NpgsqlDataReader myReader;
            using (NpgsqlConnection myCon = new NpgsqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@UserId", id);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Silme Başarılı!");
        }
    }
}
