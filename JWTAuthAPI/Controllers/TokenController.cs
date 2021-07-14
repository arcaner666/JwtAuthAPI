using JWTAuthAPI.Dtos;
using JWTAuthAPI.Models;
using JWTAuthAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWTAuthAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        readonly JWTAuthDBContext _jWTAuthDBContext;
        readonly ITokenService _tokenService;

        public TokenController(JWTAuthDBContext jWTAuthDBContext, ITokenService tokenService)
        {
            _jWTAuthDBContext = jWTAuthDBContext;
            _tokenService = tokenService;
        }

        [HttpPost("refresh")]
        public IActionResult Refresh(UserDto userDto)
        {
            if (userDto == null)
            {
                return BadRequest("Geçersiz istek!");
            }

            string accessToken = userDto.AccessToken;
            string refreshToken = userDto.RefreshToken;

            var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);
            var username = principal.Identity.Name; //this is mapped to the Name claim by default

            var user = _jWTAuthDBContext.Users.SingleOrDefault(u => u.UserName == username);
            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return BadRequest("Geçersiz istek!");
            }

            var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;

            _jWTAuthDBContext.SaveChanges();

            UserDto userResponseDto = new()
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
            };

            return Ok(userResponseDto);
        }

        [HttpPost("revoke"), Authorize]
        public IActionResult Revoke()
        {
            var username = User.Identity.Name;
            var user = _jWTAuthDBContext.Users.SingleOrDefault(u => u.UserName == username);
            if (user == null) 
            {
                return BadRequest("Geçersiz istek!");
            }
            
            user.RefreshToken = null;

            _jWTAuthDBContext.SaveChanges();

            return NoContent();
        }
    }
}
