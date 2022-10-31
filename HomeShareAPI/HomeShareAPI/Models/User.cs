namespace HomeShareAPI.Models
{
    public class User
    {
        public int UserId { get; set; }

        public User()
        {

        }

        public User(int userId, string userName, string dOB, string email, string phoneNumber, string academicFocus, string schoolYear, string personalIntroduction, string profileImageBytesString, string personalityQuestion1, string personalityQuestion2, string personalityQuestion3)
        {
            UserId = userId;
            UserName = userName;
            DOB = dOB;
            Email = email;
            PhoneNumber = phoneNumber;
            AcademicFocus = academicFocus;
            SchoolYear = schoolYear;
            PersonalIntroduction = personalIntroduction;
            ProfileImageBytesString = profileImageBytesString;
            PersonalityQuestion1 = personalityQuestion1;
            PersonalityQuestion2 = personalityQuestion2;
            PersonalityQuestion3 = personalityQuestion3;
        }

        public string UserName { get; set; }
        public string DOB { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string AcademicFocus { get; set; }
        public string SchoolYear { get; set; }
        public string PersonalIntroduction { get; set; }
        public string ProfileImageBytesString { get; set; }
        public string PersonalityQuestion1 { get; set; }
        public string PersonalityQuestion2 { get; set; }
        public string PersonalityQuestion3 { get; set; }

    }
}
