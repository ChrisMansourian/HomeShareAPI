using Microsoft.AspNetCore.Mvc;
using HomeShareAPI.Models;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;

namespace HomeShareAPI.Controllers
{
    [Route("Login")]
    public class LoginController : Controller
    {
        string connectionString = "Server=tcp:homeshare1.database.windows.net,1433;Initial Catalog=homeshare;Persist Security Info=False;User ID=admin12345;Password=root123!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        [HttpGet("CheckLogin")]
        public User Login(string username, string password)
        {
            try
            {
                User u = null;
                using (var conn = new SqlConnection(connectionString))
                using (var command = new SqlCommand("usp_Login", conn)
                {
                    CommandType = CommandType.StoredProcedure
                })
                {
                    command.Parameters.AddWithValue("username", username);
                    command.Parameters.AddWithValue("password", password);
                    conn.Open();
                    var reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        u = GetUserFromReader(reader);
                    }

                }
                return u;
            }
            catch
            {
                Debug.WriteLine("failed");
                return null;
            }
        }


        private static User GetUserFromReader(SqlDataReader reader)
        {
            int uId = int.Parse(reader["UserId"].ToString());
            string uName = reader["Username"].ToString();
            string email = reader["Email"].ToString();
            string phoneNumber = reader["PhoneNumber"].ToString();
            string DOB = reader["DOB"].ToString();
            string academicFocus = reader["AcademicFocus"].ToString();
            string schoolYear = reader["SchoolYear"].ToString();
            string personalIntroduction = reader["PersonalIntroduction"].ToString();
            byte[] imgBytes = (byte[])reader["ProfilePicture"];
            var SigBase64 = Convert.ToBase64String(imgBytes); // Get Base64
            string personalityQuestion1 = reader["PersonalityQuestion1"].ToString();
            string personalityQuestion2 = reader["PersonalityQuestion2"].ToString();
            string personalityQuestion3 = reader["PersonalityQuestion3"].ToString();

            return new User(uId, uName, DOB, email, phoneNumber, academicFocus, schoolYear, personalIntroduction, SigBase64, personalityQuestion1, personalityQuestion2, personalityQuestion3);
        }
    }
}
