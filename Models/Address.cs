using System;
using System.ComponentModel.DataAnnotations;

namespace ContactBook.Models
{
    public class Address
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        public string Street { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public int HouseNumber { get; set; }

        public User User { get; set; }
    }
}
