using Microsoft.AspNetCore.Identity;

using System;
using System.ComponentModel.DataAnnotations;

namespace ContactBook.Models
{
    public class User : IdentityUser
    {
        [Required]
        [StringLength(25, MinimumLength = 3)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(25, MinimumLength = 3)]
        public string LastName { get; set; }
    }
}
