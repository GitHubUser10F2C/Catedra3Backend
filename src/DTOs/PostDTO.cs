using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.src.DTOs
{
    public class PostDTO
    {
        public required int Id { get; set; }
        public required string Title {get; set;}
        public required string PublicationDate {get; set;}
        public required string ImageUrl { get; set; }
        public required string UserEmail { get; set; }
    }
}