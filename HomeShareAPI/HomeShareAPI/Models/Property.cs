namespace HomeShareAPI.Models
{
    public class Property
    {
        public Property()
        {

        }

        public Property(int propertyID, string streetAddress1, string streetAddress2, string city, string state, string country, int rent, int maximumCapacity, int squareFeet, double distanceToCampus, PropertyUtilities utilities)
        {
            this.propertyID = propertyID;
            this.streetAddress1 = streetAddress1;
            this.streetAddress2 = streetAddress2;
            this.city = city;
            this.state = state;
            this.country = country;
            this.rent = rent;
            this.maximumCapacity = maximumCapacity;
            this.squareFeet = squareFeet;
            this.distanceToCampus = distanceToCampus;
            this.utilities = utilities;
        }

        public int propertyID { get; set; }

        public string streetAddress1 { get; set; }
        public string streetAddress2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public int rent { get; set; }
        public int maximumCapacity { get; set; }
        public int squareFeet { get; set; }
        public double distanceToCampus { get; set; }
        public PropertyUtilities utilities { get; set; }
    }
}
