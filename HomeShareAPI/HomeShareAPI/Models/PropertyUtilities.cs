namespace HomeShareAPI.Models
{
    public class PropertyUtilities
    {
        public PropertyUtilities()
        {

        }

        public PropertyUtilities(bool pool, bool ac, bool laundry, bool dishwasher, bool balcony, bool fireplace)
        {
            this.pool = pool;
            this.ac = ac;
            this.laundry = laundry;
            this.dishwasher = dishwasher;
            this.balcony = balcony;
            this.fireplace = fireplace;
        }

        public bool pool { get; set; }
        public bool ac { get; set; }
        public bool laundry { get; set; }
        public bool dishwasher { get; set; }
        public bool balcony { get; set; }
        public bool fireplace { get; set; }
    }
}
