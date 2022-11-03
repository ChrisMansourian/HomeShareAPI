using HomeShareAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace HomeShareAPI.Controllers
{
    [Route("Invitation")]
    public class InvitationController : Controller
    {
        string connectionString = "Server=tcp:homeshare1.database.windows.net,1433;Initial Catalog=homeshare;Persist Security Info=False;User ID=admin12345;Password=root123!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        [HttpGet("GetPosts")]
        public List<Invitation> GetPosts(int userId, string sortCriteria, int ascending)
        {
            try
            {
                List<Invitation> i = new List<Invitation>();
                using (var conn = new SqlConnection(connectionString))
                using (var command = new SqlCommand("usp_getPosts", conn)
                {
                    CommandType = CommandType.StoredProcedure
                })
                {
                    command.Parameters.AddWithValue("currentDate", DateTime.Now.Date);
                    command.Parameters.AddWithValue("userId", userId);
                    command.Parameters.AddWithValue("orderBy", sortCriteria);
                    command.Parameters.AddWithValue("ascending", ascending);
                    conn.Open();
                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        i.Add(GetInvitationFromReader(reader));
                    }

                }
                return i;
            }
            catch
            {
                Debug.WriteLine("failed getting posts");
                return new List<Invitation>();
            }
        }

        private static Invitation GetInvitationFromReader(SqlDataReader reader)
        {
            int postId = int.Parse(reader["POSTID"].ToString());
            int propertyID = int.Parse(reader["PROPERTYID"].ToString());
            string streetAddress1 = reader["StreetAddress1"].ToString();
            string streetAddress2 = reader["StreetAddress2"].ToString();
            string state = reader["State"].ToString();
            string city = reader["City"].ToString();
            string country = reader["Country"].ToString();
            bool ac = reader["AC"].ToString() == "1";
            bool balcony = reader["Balcony"].ToString() == "1";
            bool dishwasher = reader["Dishwasher"].ToString() == "1";
            bool fireplace = reader["Fireplace"].ToString() == "1";
            bool laundry = reader["Laundry"].ToString() == "1";
            bool pool = reader["Pool"].ToString() == "1";
            int rent = int.Parse(reader["Rent"].ToString());
            int squareFeet = int.Parse(reader["SquareFeet"].ToString());
            int maxCap = int.Parse(reader["MaximumCapacity"].ToString());
            int numOfRoomates = int.Parse(reader["NumberOfRoomates"].ToString());
            int ownerId = int.Parse(reader["USERID"].ToString());
            double distance = double.Parse(reader["DistanceToCampus"].ToString());
            string date = reader["DOD"].ToString();
            List<string> splitQuestions = reader["InvitationQuestions"].ToString().Split(",").ToList();
            PropertyUtilities utilities = new PropertyUtilities(pool, ac, laundry, dishwasher, balcony, fireplace);
            Property property = new Property(propertyID, streetAddress1, streetAddress2, city, state, country, rent, maxCap, squareFeet, distance, utilities);
            Invitation invitation = new Invitation(postId, ownerId, property, date, numOfRoomates, splitQuestions);

            return invitation;
        }
    }
}
