using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieStore.Core.Models.Request;
using MovieStore.Core.ServiceInterfaces;
using MovieStore.Core.Models.Response;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace MovieStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private IUserService _userService;
        private readonly IConfiguration _configuration;

        public AccountController(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("register")]
        // http://localhost/api/account/register -- Http Post
        public async Task<IActionResult> RegisterUser([FromBody] UserRegisterRequestModel model)
        {
            // Model Binding
            // when posting json data, make sure your json keys are same as C# model proerties
            //{
            //    "firstName" : "Andy",
            //    "lastName" : "Wang",
            //    "email" : "andy.wang@gmail.com",
            //    "password":"Andy300#"
            //}

            // in MVC, name of the input in HTML should be same as C# model properties
            var user = await _userService.RegisterUser(model);
            return Ok(user);
        }

        [HttpPost]
        [Route("login")]
        // http://localhost/api/account/login -- Http Post
        public async Task<IActionResult> LoginUser([FromBody] LoginRequestModel model)
        {
            // Model binding
            //{
            //    "Email" : "andy.wang@gmail.com",
            //    "Password" : "Andy300#"
            //}
            var user = await _userService.ValidateUser(model.Email, model.Password);


            // only when the user is valid we need to create the JWT token abd send it to client(Angular, mobile, postmna)

            if (user == null)
            {
                return Unauthorized();
            }

            return Ok(new { token = GenerateJWT(user) });


        }

        private string GenerateJWT(UserLoginResponseModel user)
        {
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.GivenName, user.FirstName),
                    new Claim(ClaimTypes.Surname, user.LastName),
                    new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                    new Claim(ClaimTypes.Email,  user.Email),
                };
            // send these information into payload, claims

            var identityClaims = new ClaimsIdentity();
            identityClaims.AddClaims(claims);

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["TokenSettings:PrivateKey"])); //getting privateKey from appsetting
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature); //Algorithm
            var expires = DateTime.UtcNow.AddHours(_configuration.GetValue<double>("TokenSettings:ExpirationHours"));

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identityClaims,
                Expires = expires,
                SigningCredentials = credentials,
                Issuer = _configuration["TokenSettings:Issuer"],
                Audience = _configuration["TokenSettings:Audience"]
            };
            var encodedJwt = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(encodedJwt);
        }
    }
}
