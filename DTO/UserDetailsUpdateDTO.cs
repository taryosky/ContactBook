using System;
using System.ComponentModel.DataAnnotations;

namespace ContactBook.DTO
{
    public class UserDetailsUpdateDTO
    {
        [Required]
        public string Id { get; set; }

        [Required]
        [StringLength(25, MinimumLength = 3)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(25, MinimumLength = 3)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(6)]
        public string Password { get; set; }

        public AddressDTO Address { get; set; }
    }
}
