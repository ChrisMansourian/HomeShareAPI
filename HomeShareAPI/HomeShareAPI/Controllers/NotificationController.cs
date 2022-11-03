using HomeShareAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace HomeShareAPI.Controllers
{
    [Route("Notification")]
    public class NotificationController : Controller
    {

        string connectionString = "Server=tcp:homeshare1.database.windows.net,1433;Initial Catalog=homeshare;Persist Security Info=False;User ID=admin12345;Password=root123!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        
        [HttpGet("CreateNewNotification")]
        public bool CreateNotification(int userId, int postId, string text)
        {

            try
            {
                using (var conn = new SqlConnection(connectionString))
                using (var command = new SqlCommand("usp_createNotification", conn)
                {
                    CommandType = CommandType.StoredProcedure
                })
                {
                    command.Parameters.AddWithValue("userId", userId);
                    command.Parameters.AddWithValue("postId", postId);
                    command.Parameters.AddWithValue("notification", text);

                    conn.Open();
                    command.ExecuteNonQuery();

                    return true;
                }
            }
            catch
            {
                Debug.WriteLine("failed creating notification");
                return false;
            }
        }


        [HttpGet("GetNotifications")]
        public List<Notifications> GetNotifications(int userId)
        {
            try
            {
                List<Notifications> n = new List<Notifications>();
                using (var conn = new SqlConnection(connectionString))
                using (var command = new SqlCommand("usp_getNotifications", conn)
                {
                    CommandType = CommandType.StoredProcedure
                })
                {
                    command.Parameters.AddWithValue("userId", userId);
                    conn.Open();
                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        n.Add(GetNotificationFromReader(reader));
                    }

                }
                return n;
            }
            catch
            {
                Debug.WriteLine("failed getting that notifications");
                return new List<Notifications>();
            }
        }

        private static Notifications GetNotificationFromReader(SqlDataReader reader)
        {
            int uId = int.Parse(reader["USERID"].ToString());
            int pId = int.Parse(reader["POSTID"].ToString());
            int notified;
            if (reader["Notified"].ToString() == "")
            {
                notified = -1;
            }
            else
            {
                notified = int.Parse(reader["Notified"].ToString());
            }
            string notification = reader["Notification"].ToString();

            return new Notifications(uId, pId, notified, notification);
        }
    }
}
