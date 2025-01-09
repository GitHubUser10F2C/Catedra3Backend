using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using backend.src.DTOs;
using backend.src.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.src.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        private readonly IPostRepository _postRepository;
        public PostController(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        /// <summary>
        /// Obtiene una lista de posts.
        /// </summary>
        /// <returns>Una lista de posts.</returns>
        /// <response code="200">Devuelve la lista de posts.</response>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var posts = await _postRepository.GetAll();
            return Ok(posts);
        }

        /// <summary>
        /// Agrega un nuevo post al sistema.
        /// </summary>
        /// <param name="postDto">Datos necesarios para agregar un nuevo post.</param>
        /// <returns>El post agregado con su ID asignado.</returns>
        /// <response code="201">Devuelve el post creado.</response>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddPost(AddPostDTO postDto)
        {
            var appUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            try
            {
                var addedPost = await _postRepository.AddPost(postDto, appUserId!);
                return CreatedAtAction(nameof(GetById), new { id = addedPost.Id }, addedPost);
            }
            catch (ArgumentException ex)
            {
                // Errores de validación de la imagen
                return BadRequest(new { message = ex.Message});
            }
            catch (InvalidOperationException ex)
            {
                // Error de carga en Cloudinary
                return StatusCode(StatusCodes.Status503ServiceUnavailable, new { message = ex.Message});
            }
            catch (KeyNotFoundException ex)
            {
                // El usuario no se encuentra
                return NotFound(new { message = ex.Message});
            }
        }

        /// <summary>
        /// Obtiene un post por su ID.
        /// </summary>
        /// <param name="id">El ID del post que se desea obtener.</param>
        /// <returns>El post correspondiente al ID proporcionado.</returns>
        /// <response code="200">Devuelve el post encontrado.</response>
        /// <response code="404">Si el post no se encuentra con el ID proporcionado.</response>
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            var post = await _postRepository.GetById(id);
            if (post == null)
            {
                return NotFound(new { message = $"Post with id {id} not found."});
            }

            return Ok(post);
        }
    }
}