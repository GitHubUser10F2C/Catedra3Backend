using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace backend.src.DTOs
{
    public class RegisterDTO
    {
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public required string Email { get; set; }
        [Required]
        [StringLength(32, MinimumLength = 6, ErrorMessage = "The password must be between 6 and 32 characters.")]
        [RegularExpression(@"^.*\d.*$", ErrorMessage = "The password must contain at least one number.")]
        public required string Password { get; set; }
    }
}