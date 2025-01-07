using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using backend.src.DTOs;
using backend.src.Interfaces;
using backend.src.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace backend.src.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public AuthController(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        /// <summary>
        /// Registra un nuevo usuario en el sistema.
        /// </summary>
        /// <param name="registerDto">Datos necesarios para registrar el usuario</param>
        /// <returns>Resultado del registro de usuario, con mensaje de éxito o error.</returns>
        /// <response code="200">Devuelve un mensaje de éxito al registrar al usuario.</response>
        /// <response code="400">Si el correo electrónico ya están registrados o si hay algún error durante el registro.</response>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDto)
        {

            // Validar que el correo electrónico no exista
            var existingUserByEmail = await _userRepository.GetByEmail(registerDto.Email);
            if (existingUserByEmail != null)
            {
                return BadRequest("Email already registered.");
            }

            var user = new AppUser
            {
                UserName = registerDto.Email,
                Email = registerDto.Email,
            };

            var result = await _userRepository.CreateUser(user, registerDto.Password);

            if (result.Succeeded)
            {
                return Ok("User registered successfully.");
            }

            return BadRequest(result.Errors);
        }

        /// <summary>
        /// Inicia sesión de un usuario autenticado y genera un token JWT.
        /// </summary>
        /// <param name="loginDto">Datos de inicio de sesión, que incluyen el correo electrónico y la contraseña del usuario.</param>
        /// <returns>Token JWT si las credenciales son correctas.</returns>
        /// <response code="200">Devuelve el token JWT si las credenciales son correctas.</response>
        /// <response code="401">Si las credenciales son incorrectas, devuelve un error de autorización.</response>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            var user = await _userRepository.GetByEmail(loginDto.Email);
            if (user == null)
            {
                return Unauthorized("Incorrect email or password.");
            }

            var isPasswordValid = await _userRepository.CheckPassword(user, loginDto.Password);
            if (!isPasswordValid)
            {
                return Unauthorized("Incorrect password or email.");
            }

            var token = GenerateJwtToken(user);
            return Ok(new { token });
        }

        /// <summary>
        /// Genera un token JWT para el usuario autenticado.
        /// </summary>
        /// <param name="user">El usuario para el cual se generará el token.</param>
        /// <returns>El token JWT generado.</returns>
        private string GenerateJwtToken(AppUser user)
        {
            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.UserName!),
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SigninKey"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _configuration["JWT:Issuer"],
                _configuration["JWT:Audience"],
                claims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}