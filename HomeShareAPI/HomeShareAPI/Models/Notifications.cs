namespace HomeShareAPI.Models
{
    public class Notifications
    {
        public int USERID { get; set; }
        public int POSTID { get; set; }
        public int Notified { get; set; }
        public string Notification { get; set; }

        public Notifications()
        {
            Notification = "";
        }

        public Notifications(int uSERID, int pOSTID, int notified, string notification)
        {
            USERID = uSERID;
            POSTID = pOSTID;
            Notified = notified;
            Notification = notification;
        }
    }
}
