using Microsoft.AspNetCore.Http;

using System;
using System.ComponentModel.DataAnnotations;

namespace ContactBook.DTO
{
    public class FileToUploadDTO
    {
        [Required]
        public IFormFile File { get; set; }
    }
}
