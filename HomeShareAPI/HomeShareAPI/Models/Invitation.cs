namespace HomeShareAPI.Models
{
    public class Invitation
    {
        public Invitation(int postId, int userId, Property property, string dateOfDeadline, int numOfRoomates, List<string> Questions)
        {
            this.postId = postId;
            this.userId = userId;
            this.property = property;
            this.dateOfDeadline = dateOfDeadline;
            this.numOfRoomates = numOfRoomates;
            this.Questions = Questions;
            this.numOfRoomates = numOfRoomates;
        }
        public Invitation()
        {

        }
        public int postId { get; set; }
        public int userId { get; set; }
        public string dateOfDeadline { get; set; }
        
        public int numOfRoomates { get; set; }
        public List<string> Questions { get; set; }

        public Property property { get; set; }

        public List<User> roomates { get; set; }

        public List<Responses> responses { get; set; }

    }
}
