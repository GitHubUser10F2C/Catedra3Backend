using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.src.DTOs;
using backend.src.Models;

namespace backend.src.Mappers
{
    public static class PostMapper
    {
        public static PostDTO ToPostDto(this Post postModel)
        {
            return new PostDTO
            {
                Id = postModel.Id,
                Title = postModel.Title,
                PublicationDate = postModel.PublicationDate.ToString("dd/MM/yyyy HH:mm"),
                ImageUrl = postModel.ImageUrl,
                UserEmail = postModel.AppUser.Email!
            };
        }
    }
}