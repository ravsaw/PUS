namespace PUS.Models
{
    public class Address
    {
        public int Id { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public int Zipcode { get; set; }
        public string City { get; set; }
        public string Print { get
            {
                return Zipcode + " " + City + ", " + Address1 + " " + Address2;
            }
        }
    }
}
