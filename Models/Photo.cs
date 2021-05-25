using System;
using System.ComponentModel.DataAnnotations;

namespace ContactBook.Models
{
    public class Photo
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [Required]
        public string PublicKey { get; set; }

        public User User { get; set; }
    }
}
