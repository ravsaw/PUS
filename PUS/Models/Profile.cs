﻿namespace PUS.Models
{
    public class Profile
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }

        public ICollection<Address> Addresses { get; set; }
        public ICollection<Company> Companies { get; set; }
    }
}