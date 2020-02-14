using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Dziekanat.Entities;
using Dziekanat.Helpers;
using Dziekanat.Models;
using Dziekanat.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Dziekanat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {

        private readonly EmployeeContext _employeeContext;
        private readonly StudentContext _studentContext;
        private IAuthenticationService _authenticationService;
        private readonly AppSettings _appSettings;

        public AuthenticationController(EmployeeContext employeeContext, StudentContext studentContext,
            IAuthenticationService authenticationService, IOptions<AppSettings> appSettings)
        {
            _employeeContext = employeeContext;
            _studentContext = studentContext;
            _authenticationService = authenticationService;
            _appSettings = appSettings.Value;
        }

        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]AuthenticateModel model)
        {

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

            if (model.StudentOrEmployee == "Employee")
            {
                var user = _authenticationService.Authenticate(model.Email, model.Password, _employeeContext);

                if (user == null)
                    return BadRequest(new { message = "Username or password is incorrect" });

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, user.Employee_Id.ToString()),
                        new Claim(ClaimTypes.Role, user.Role.ToString()),
                    }),
                    Expires = DateTime.UtcNow.AddMinutes(30),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                // return basic user info and authentication token
                return Ok(new
                {
                    Id = user.Employee_Id,
                    First_Name = user.First_Name,
                    Last_Name = user.Last_Name,
                    Token = tokenString
                });

            }
            else if (model.StudentOrEmployee == "Student")
            {
                var user = _authenticationService.Authenticate(model.Email, model.Password, _studentContext);
                if (user == null)
                    return BadRequest(new { message = "Username or password is incorrect" });

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, user.Student_Id.ToString()),
                        new Claim(ClaimTypes.Role, user.Role)
                    }),
                    Expires = DateTime.UtcNow.AddMinutes(30),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                // return basic user info and authentication token
                return Ok(new
                {
                    Id = user.Student_Id,
                    First_Name = user.First_Name,
                    Last_Name = user.Last_Name,
                    Token = tokenString
                });

            }
            else return BadRequest(new { message = "Are you student or employee?" });



        }

    }
}