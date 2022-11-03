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
                Debug.WriteLine("failed login");
                return null;
            }
        }

        [HttpGet("SignUp")]
        public bool CreateAccount(string username, string password, string dob, string email, string number,
                                        string academicFocus, string schoolYear, string personalIntro, string img,
                                        string personalityQuestion1, string personalityQuestion2, string personalityQuestion3)
        {

            try
            {
                User u = null;
                using (var conn = new SqlConnection(connectionString))
                using (var command = new SqlCommand("usp_signUp", conn)
                {
                    CommandType = CommandType.StoredProcedure
                })
                {
                    command.Parameters.AddWithValue("username", username);
                    command.Parameters.AddWithValue("pass", password);
                    command.Parameters.AddWithValue("dob", DateTime.Parse(dob));
                    command.Parameters.AddWithValue("email", email);
                    command.Parameters.AddWithValue("number", number);
                    command.Parameters.AddWithValue("major", academicFocus);
                    command.Parameters.AddWithValue("yearOfGrad", schoolYear);
                    command.Parameters.AddWithValue("personalIntro", personalIntro);
                    byte[] imgBytes = img != null ? Convert.FromBase64String(img) : new byte[0];
                    command.Parameters.AddWithValue("image", imgBytes != null ? imgBytes : new byte[0]);
                    command.Parameters.AddWithValue("personalityQuestion1", personalityQuestion1);
                    command.Parameters.AddWithValue("personalityQuestion2", personalityQuestion2);
                    command.Parameters.AddWithValue("personalityQuestion3", personalityQuestion3);

                    conn.Open();
                    command.ExecuteNonQuery();

                    return true;
                }
            }
            catch
            {
                Debug.WriteLine("failed sign up");
                return false;
            }
        }

        [HttpGet("GetUser")]
        public User GetUser(int userId)
        {
            try
            {
                User u = null;
                using (var conn = new SqlConnection(connectionString))
                using (var command = new SqlCommand("usp_getUser", conn)
                {
                    CommandType = CommandType.StoredProcedure
                })
                {
                    command.Parameters.AddWithValue("userId", userId);
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
                Debug.WriteLine("failed getting that user");
                return null;
            }
        }
        
        [HttpGet("GetUserByName")]
        public User GetUser(string username)
        {
            try
            {
                User u = null;
                using (var conn = new SqlConnection(connectionString))
                using (var command = new SqlCommand("usp_getUserByName", conn)
                {
                    CommandType = CommandType.StoredProcedure
                })
                {
                    command.Parameters.AddWithValue("username", username);
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
                Debug.WriteLine("failed getting that user");
                return null;
            }
        }

        [HttpGet("ChangeUserName")]
        public bool ChangeUserName(int userId, string username)
        {

            try
            {
                User u = null;
                using (var conn = new SqlConnection(connectionString))
                using (var command = new SqlCommand("usp_changeUserName", conn)
                {
                    CommandType = CommandType.StoredProcedure
                })
                {
                    command.Parameters.AddWithValue("userId", userId);
                    command.Parameters.AddWithValue("userName", username);

                    conn.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    bool result = false;

                    if (reader.Read())
                    {
                        int res = int.Parse(reader["result"].ToString());
                        if (res == 1)
                        {
                            result = true;
                        }
                    }

                    return result;
                }
            }
            catch
            {
                Debug.WriteLine("failed changing the username");
                return false;
            }
        }

        [HttpGet("CheckUserNameExists")]
        public bool CheckUserNameExists(string username)
        {

            try
            {
                using (var conn = new SqlConnection(connectionString))
                using (var command = new SqlCommand("usp_checkUserNameExists", conn)
                {
                    CommandType = CommandType.StoredProcedure
                })
                {
                    command.Parameters.AddWithValue("userName", username);

                    conn.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    bool result = false;

                    if (reader.Read())
                    {
                        int res = int.Parse(reader["result"].ToString());
                        if (res == 1)
                        {
                            result = true;
                        }
                    }

                    return result;
                }
            }
            catch
            {
                Debug.WriteLine("failed changing the username");
                return false;
            }
        }

        [HttpGet("UpdateProfile")]
        public bool UpdateProfile(string username, string dob, string email, string number,
                                        string academicFocus, string schoolYear, string personalIntro, string img,
                                        string personalityQuestion1, string personalityQuestion2, string personalityQuestion3)
        
        {
            if (personalityQuestion1 == null)
            {
                personalityQuestion1 = "";
            }
            if (personalityQuestion2 == null)
            {
                personalityQuestion2 = "";
            }
            if (personalityQuestion3 == null)
            {
                personalityQuestion3 = "";
            }

            try
            {
                using (var conn = new SqlConnection(connectionString))
                using (var command = new SqlCommand("usp_updateProfile", conn)
                {
                    CommandType = CommandType.StoredProcedure
                })
                {
                    command.Parameters.AddWithValue("username", username);
                    command.Parameters.AddWithValue("dob", DateTime.Parse(dob));
                    command.Parameters.AddWithValue("email", email);
                    command.Parameters.AddWithValue("number", number);
                    command.Parameters.AddWithValue("major", academicFocus);
                    command.Parameters.AddWithValue("yearOfGrad", schoolYear);
                    command.Parameters.AddWithValue("personalIntro", personalIntro);
                    byte[] imgBytes = img != null ? Convert.FromBase64String(img) : new byte[0];
                    command.Parameters.AddWithValue("image", imgBytes != null ? imgBytes : new byte[0]);
                    command.Parameters.AddWithValue("personalityQuestion1", personalityQuestion1);
                    command.Parameters.AddWithValue("personalityQuestion2", personalityQuestion2);
                    command.Parameters.AddWithValue("personalityQuestion3", personalityQuestion3);

                    conn.Open();
                    command.ExecuteNonQuery();

                    return true;
                }
            }
            catch
            {
                Debug.WriteLine("failed sign up");
                return false;
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
