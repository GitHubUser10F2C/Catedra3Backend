using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.src.DTOs;

namespace backend.src.Interfaces
{
    public interface IPostRepository
    {
        Task<List<PostDTO>> GetAll();
        Task<PostDTO> AddPost(AddPostDTO postDTO, string appUserId);
        Task<PostDTO?> GetById(int id);
    }
}