namespace ThaiTable.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Customer
    {
        public int ID { get; set; }
        [Required(ErrorMessage="*")]
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string City { get; set; }

        public string Postcode { get; set; }
        public string PhoneNo { get; set; }
        public string Info { get; set; }

        public virtual Order Order { get; set; }

        public string FullName { get { return FirstName + " " + LastName; } }
    }
}
