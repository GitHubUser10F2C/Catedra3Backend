using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Net.Http.Headers;

namespace backend.src.Models
{
    public class Post
    {
        public int Id { get; set; }
        public required string Title {get; set;}
        public required DateTime PublicationDate {get; set;}
        public required string ImageUrl { get; set; }
        public required string AppUserId { get; set; }
        public required AppUser AppUser { get; set; }
    }
}