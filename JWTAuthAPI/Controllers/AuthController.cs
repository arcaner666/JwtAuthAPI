using JWTAuthAPI.Dtos;
using JWTAuthAPI.Models;
using JWTAuthAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JWTAuthAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        readonly JWTAuthDBContext _jWTAuthDBContext;
        readonly ITokenService _tokenService;

        public AuthController(JWTAuthDBContext jWTAuthDBContext, ITokenService tokenService)
        {
            _jWTAuthDBContext = jWTAuthDBContext;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public IActionResult Login(UserDto userDto)
        {
            if (userDto == null)
            {
                return BadRequest("Geçersiz istek!");
            }

            User user = _jWTAuthDBContext.Users.FirstOrDefault(u =>
            u.UserName == userDto.UserName && u.Password == userDto.Password);

            if (user == null)
            {
                return Unauthorized("Kullanıcı adı veya parola yanlış!");
            }

            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, user.UserName));
            //if (user.UserName == "caner")
            //{
            //    claims.Add(new Claim(ClaimTypes.Role, "User"));
            //} else
            if (user.UserName == "admin")
            {
                claims.Add(new Claim(ClaimTypes.Role, "Admin"));
            }

            var accessToken = _tokenService.GenerateAccessToken(claims);
            var refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddMinutes(5);

            _jWTAuthDBContext.SaveChanges();

            UserDto userResponseDto = new()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };

            return Ok(userResponseDto);
        }
    }
}
