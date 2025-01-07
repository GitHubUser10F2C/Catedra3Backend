using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace backend.src.DTOs
{
    public class LoginDTO
    {
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public required string Email { get; set; }
        [Required]
        public required string Password { get; set; }
    }
}