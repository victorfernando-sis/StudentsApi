using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AlunosApi.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using StudentsApi.Services;

namespace StudentsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IAuthenticate _authentication;

        public AccountController(IConfiguration configuration, IAuthenticate authentication)
        {
            _configuration = configuration ??
                throw new ArgumentNullException(nameof(configuration));

            _authentication = authentication ??
                throw new ArgumentNullException(nameof(authentication));
        }

        [HttpPost("CreateUser")]
        public async Task<ActionResult<UserToken>> CreateUser([FromBody] RegisterModel model)
        {
            if (model.Password != model.ConfirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword", "Passwords do not match");
                return BadRequest(ModelState);
            }
            var result = await _authentication.RegisterUser(model.Email, model.Password);
            if (result)
            {
                return Ok($"User {model.Email} created successfully");
            }
            else
            {
                ModelState.AddModelError("CreateUser", "Invalid register.");
                return BadRequest(ModelState);
            }
        }
        [HttpPost("LoginUser")]
        public async Task<ActionResult<UserToken>> LoginUser([FromBody] LoginModel userInfo)
        {
            var result = await _authentication.Authenticate(userInfo.Email, userInfo.Password);

            if (result)
            {
                return GenerateToken(userInfo);
            }
            else
            {
                ModelState.AddModelError("LoginUser", "Login inválido.");
                return BadRequest(ModelState);
            }
        }

        private ActionResult<UserToken> GenerateToken(LoginModel userInfo)
        {
            var claims = new[]
            {
                new Claim("email", userInfo.Email),
                new Claim("meuToken", "token do macoratti"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:key"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.UtcNow.AddMinutes(20);

            JwtSecurityToken token = new JwtSecurityToken(
               issuer: _configuration["Jwt:Issuer"],
               audience: _configuration["Jwt:Audience"],
               claims: claims,
               expires: expiration,
               signingCredentials: creds);

            return new UserToken()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration,
            };
        }
    }
}