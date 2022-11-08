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

        [HttpGet("GetInvitationForPosterFromID")]
        public Invitation GetPostsFromID(int userId)
        {
            try
            {
                Invitation i = null;
                using (var conn = new SqlConnection(connectionString))
                using (var command = new SqlCommand("usp_getPost", conn)
                {
                    CommandType = CommandType.StoredProcedure
                })
                {
                    command.Parameters.AddWithValue("currentDate", DateTime.Now.Date);
                    command.Parameters.AddWithValue("userId", userId);
                    conn.Open();
                    var reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        i = GetInvitationFromReader(reader);

                        i.responses = GetResponses(i.postId);
                        i.roomates = GetRoomates(i.postId);
                    }

                }
                return i;
            }
            catch
            {
                Debug.WriteLine("failed getting post");
                return null;
            }
        }

        [HttpGet("GetPostOwner")]
        public int GetPostOwner(int postId)
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                using (var command = new SqlCommand("usp_getUserFromPost", conn)
                {
                    CommandType = CommandType.StoredProcedure
                })
                {
                    command.Parameters.AddWithValue("postId", postId);
                    conn.Open();
                    var reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        return int.Parse(reader["USERID"].ToString());
                    }

                }
                return -1;
            }
            catch
            {
                Debug.WriteLine("failed getting post owner");
                return 0;
            }
        }

        [HttpGet("GetResponses")]
        public List<Responses> GetResponses(int postId)
        {
            try
            {
                List<Responses> list = new List<Responses>();
                using (var conn = new SqlConnection(connectionString))
                using (var command = new SqlCommand("usp_getResponses", conn)
                {
                    CommandType = CommandType.StoredProcedure
                })
                {
                    command.Parameters.AddWithValue("postId", postId);
                    conn.Open();
                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        int userId = int.Parse(reader["USERID"].ToString());
                        List<string> qR = GetQuestionResponses(userId, postId);
                        User u = new LoginController().GetUser(userId);
                        list.Add(new Responses() { questionResponses = qR, user = u });
                    }

                }
                return list;
            }
            catch
            {
                Debug.WriteLine("failed getting responses");
                return null;
            }
        }

        [HttpGet("GetUserQuestionResponses")]
        public List<string> GetQuestionResponses(int userId, int postId)
        {
            try
            {
                List<string> list = new List<string>();
                using (var conn = new SqlConnection(connectionString))
                using (var command = new SqlCommand("usp_getQuestionResponses", conn)
                {
                    CommandType = CommandType.StoredProcedure
                })
                {
                    command.Parameters.AddWithValue("postId", postId);
                    command.Parameters.AddWithValue("userId", userId);
                    conn.Open();
                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        string response = reader["QuestionsResponse"].ToString();
                        list.Add(response);
                    }

                }
                return list;
            }
            catch
            {
                Debug.WriteLine("failed getting question responses");
                return null;
            }
        }

        [HttpGet("GetRoomates")]
        public List<User> GetRoomates(int postId)
        {
            try
            {
                List<User> list = new List<User>();
                using (var conn = new SqlConnection(connectionString))
                using (var command = new SqlCommand("usp_getCurrentRoomates", conn)
                {
                    CommandType = CommandType.StoredProcedure
                })
                {
                    command.Parameters.AddWithValue("postId", postId);
                    conn.Open();
                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        int userId = int.Parse(reader["USERID"].ToString());
                        User u = new LoginController().GetUser(userId);
                        list.Add(u);
                    }

                }
                return list;
            }
            catch
            {
                Debug.WriteLine("failed getting roommates");
                return null;
            }
        }

        [HttpPost("CreateNewInvitation")]
        public bool CreateNewInvitation([FromBody]Invitation invitation)
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                using (var command = new SqlCommand("usp_createPost", conn)
                {
                    CommandType = CommandType.StoredProcedure
                })
                {
                    command.Parameters.AddWithValue("userId", invitation.userId);
                    command.Parameters.AddWithValue("streetAddress1", invitation.property.streetAddress1);
                    command.Parameters.AddWithValue("streetAddress2", invitation.property.streetAddress2 == null ? "" : invitation.property.streetAddress2);
                    command.Parameters.AddWithValue("city", invitation.property.city);
                    command.Parameters.AddWithValue("state", invitation.property.state);
                    command.Parameters.AddWithValue("country", invitation.property.country);
                    command.Parameters.AddWithValue("squarefeet", invitation.property.squareFeet);
                    command.Parameters.AddWithValue("distanceToCampus", invitation.property.distanceToCampus);
                    command.Parameters.AddWithValue("DOD", DateTime.Parse(invitation.dateOfDeadline));
                    command.Parameters.AddWithValue("maximumCapacity", invitation.property.maximumCapacity);
                    command.Parameters.AddWithValue("rent", invitation.property.rent);
                    command.Parameters.AddWithValue("pool", invitation.property.utilities.pool);
                    command.Parameters.AddWithValue("ac", invitation.property.utilities.ac);
                    command.Parameters.AddWithValue("laundry", invitation.property.utilities.laundry);
                    command.Parameters.AddWithValue("dishwasher", invitation.property.utilities.dishwasher);
                    command.Parameters.AddWithValue("balcony", invitation.property.utilities.balcony);
                    command.Parameters.AddWithValue("fireplace", invitation.property.utilities.fireplace);
                    command.Parameters.AddWithValue("bedrooms", invitation.property.bedrooms);
                    command.Parameters.AddWithValue("bathrooms", invitation.property.bathrooms);
                    conn.Open();
                    var reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        int created = int.Parse(reader["created"].ToString());
                        if (created == 1)
                        {
                            int postId = int.Parse(reader["result"].ToString());

                            AddQuestions(postId, invitation.Questions);

                            return true;
                        }
                    }

                }
                return false;
            }
            catch (Exception e)
            {
                Debug.WriteLine("failed creating invitation");
                return false;
            }
        }


        [HttpGet("ManageResponse")]
        public bool ManageResponse(int postId, int userId, int posterResponse)
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                using (var command = new SqlCommand("usp_managePostResponse", conn)
                {
                    CommandType = CommandType.StoredProcedure
                })
                {
                    command.Parameters.AddWithValue("postId", postId);
                    command.Parameters.AddWithValue("userId", userId);
                    command.Parameters.AddWithValue("posterResponse", posterResponse);
                    conn.Open();
                    command.ExecuteNonQuery();

                }
                return true;
            }
            catch
            {
                Debug.WriteLine("failed managing response");
                return false;
            }
        }

        [HttpPost("AddResponse")]
        public bool AddResponse(int postId, int userId, int response, [FromBody] List<string> responses)
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                using (var command = new SqlCommand("usp_addResponse", conn)
                {
                    CommandType = CommandType.StoredProcedure
                })
                {
                    command.Parameters.AddWithValue("postId", postId);
                    command.Parameters.AddWithValue("userId", userId);
                    command.Parameters.AddWithValue("response", response);
                    conn.Open();
                    command.ExecuteNonQuery();

                    AddQuestionResponses(postId, userId, responses);

                    int ownerId = GetPostOwner(postId);
                    new NotificationController().CreateNotification(ownerId, postId, "You have one response from user " + new LoginController().GetUser(userId).UserName + "!");

                }
                return true;
            }
            catch
            {
                Debug.WriteLine("failed responding to invitation");
                return false;
            }
        }

        [HttpPost("AddQuestions")]
        public bool AddQuestions(int postId, [FromBody]List<string> questions)
        {
            for (int i = 0; i < questions.Count; i++)
            {
                try
                {
                    using (var conn = new SqlConnection(connectionString))
                    using (var command = new SqlCommand("usp_addQuestion", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    })
                    {
                        command.Parameters.AddWithValue("postId", postId);
                        command.Parameters.AddWithValue("questionNumber", i + 1);
                        command.Parameters.AddWithValue("question", questions[i]);
                        conn.Open();
                        command.ExecuteNonQuery();
                    }
                }
                catch
                {
                    Debug.WriteLine("failed adding questions");
                    return false;
                }
            }
            return true;
        }

        [HttpPost("AddQuestionResponses")]
        public bool AddQuestionResponses(int postId, int userId, [FromBody] List<string> questionResponse)
        {
            for (int i = 0; i < questionResponse.Count; i++)
            {
                try
                {
                    using (var conn = new SqlConnection(connectionString))
                    using (var command = new SqlCommand("usp_addQuestionResponse", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    })
                    {
                        command.Parameters.AddWithValue("postId", postId);
                        command.Parameters.AddWithValue("questionNum", i + 1);
                        command.Parameters.AddWithValue("userId", userId);
                        command.Parameters.AddWithValue("questionResponse", questionResponse[i]);
                        conn.Open();
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("failed adding questions responses");
                    return false;
                }
            }
            return true;
        }

        [HttpGet("DeleteRoomates")]
        public bool DeleteRoomates(int propertyId)
        {
            using (var conn = new SqlConnection(connectionString))
            using (var command = new SqlCommand("DELETE FROM RoomateTable Where PROPERTYID = @PropertyId", conn)
            {
                CommandType = CommandType.Text
            })
            {
                conn.Open();
                command.Parameters.AddWithValue("@PropertyId", propertyId);
                command.ExecuteNonQuery();
            }
            return true;
        }



        [HttpGet("DeletePost")]
        public bool DeletePost(int postId)
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                using (var command = new SqlCommand("usp_deletePost", conn)
                {
                    CommandType = CommandType.StoredProcedure
                })
                {
                    command.Parameters.AddWithValue("postId", postId);
                    conn.Open();

                    command.ExecuteNonQuery();
                }
                return true;
            }
            catch
            {
                Debug.WriteLine("failed to delete post");
                return false;
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
            double bath = double.Parse(reader["Bathrooms"].ToString());
            int beds = int.Parse(reader["Bedrooms"].ToString());
            List<string> splitQuestions = reader["InvitationQuestions"].ToString().Split(",").ToList();
            PropertyUtilities utilities = new PropertyUtilities(pool, ac, laundry, dishwasher, balcony, fireplace);
            Property property = new Property(propertyID, streetAddress1, streetAddress2, city, state, country, rent, maxCap, squareFeet, distance, utilities, bath, beds);
            Invitation invitation = new Invitation(postId, ownerId, property, date, numOfRoomates, splitQuestions);

            return invitation;
        }
    }
}
