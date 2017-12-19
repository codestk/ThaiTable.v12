using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ThaiTable.Models
{
    public class DeliveryViewModel
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string ContactNo { get; set; }

        public string FullName { get { return FirstName + " " + LastName; } }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Postcode { get; set; }
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
    }
}