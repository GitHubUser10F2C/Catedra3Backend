using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace backend.src.DTOs
{
    public class AddPostDTO
    {
        [Required]
        [StringLength(255, MinimumLength = 5, ErrorMessage = "The title must be between 5 and 255 characters.")]
        public required string Title { get; set; }
        [Required]
        public IFormFile Image { get; set; } = null!;
    }
}