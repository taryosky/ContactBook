﻿using System;
using System.ComponentModel.DataAnnotations;

namespace ContactBook.DTO
{
    public class AddressDTO
    {
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
    }
}
