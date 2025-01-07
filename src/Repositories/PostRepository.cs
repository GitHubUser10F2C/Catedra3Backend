using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.src.Data;
using backend.src.DTOs;
using backend.src.Interfaces;
using backend.src.Mappers;
using backend.src.Models;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace backend.src.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly AppDbContext _context;
        private readonly Cloudinary _cloudinary;
        private readonly UserManager<AppUser> _userManager;
        public PostRepository(AppDbContext context, Cloudinary cloudinary, UserManager<AppUser> userManager)
        {
            _context = context;
            _cloudinary = cloudinary;
            _userManager = userManager;
        }
        public async Task<PostDTO> AddPost(AddPostDTO postDTO, string appUserId)
        {
            var appUser = await _userManager.FindByIdAsync(appUserId);
            if (appUser == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            if (postDTO.Image == null || postDTO.Image.Length == 0)
            {
                throw new ArgumentException("Image is required.");
            }

            if (postDTO.Image.ContentType != "image/jpeg" && postDTO.Image.ContentType != "image/png")
            {
                throw new ArgumentException("Image must be a jpeg or png");
            }

            if (postDTO.Image.Length > 5 * 1024 * 1024)
            {
                throw new ArgumentException("Image must be less than 5mb");
            }

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(postDTO.Image.FileName, postDTO.Image.OpenReadStream()),
                Folder = "posts_image"
            };

            var uploadReulst = await _cloudinary.UploadAsync(uploadParams);

            if (uploadReulst.Error != null)
            {
                throw new InvalidOperationException($"Cloudinary upload failed: {uploadReulst.Error.Message}");
            }

            var newPost = new Post
            {
                Title = postDTO.Title,
                PublicationDate = DateTime.Now,
                ImageUrl = uploadReulst.SecureUrl.AbsoluteUri,
                AppUserId = appUserId,
                AppUser = appUser

            };

            _context.Posts.Add(newPost);
            await _context.SaveChangesAsync();

            return newPost.ToPostDto();
        }

        public async Task<List<PostDTO>> GetAll()
        {
            var posts = await _context.Posts.Include(p => p.AppUser).ToListAsync();

            return posts.Select(p => p.ToPostDto()).ToList();
        }

        public async Task<PostDTO?> GetById(int id)
        {
            var product = await _context.Posts
                .Include(p => p.AppUser)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return null;
            }

            return product.ToPostDto();
        }
    }
}