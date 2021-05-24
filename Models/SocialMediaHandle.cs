using System;
using System.ComponentModel.DataAnnotations;

namespace ContactBook.Models
{
    public class SocialMediaHandle
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public string Url { get; set; }

        public User User { get; set; }
    }
}
