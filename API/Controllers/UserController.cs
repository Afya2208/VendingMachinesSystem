using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Models.Dto;
using Models.Entities;

namespace API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("users")]
    public class UserController(VendingDbContext context, ITokenService tokenService) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            return Ok(await context.Users.ToListAsync());
        }

        [HttpPost("/sign-in")]
        [AllowAnonymous]
        public async Task<IActionResult> SignIn([FromBody] AuthorizationRequest authRequest)
        {
            User? user = await context.Users.AsNoTracking()
                .Include(x => x.Role)
                .FirstOrDefaultAsync(x => x.Email == authRequest.Email);
            if (user == null)
            {
                return Unauthorized("Неправильный пароль или логин");
            }
            else
            {
                var sha256 = SHA256.Create();
                var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(authRequest.Password));
                var stringHash = Encoding.UTF8.GetString(hash);
                if (stringHash == user.Password)
                {
                    var claimsIdentity = tokenService.GetClaimsIdentity(user.Email, user.Role.Name);
                    var token = tokenService.CreateJwtToken(claimsIdentity);
                    var authAnswer = new AuthorizationResponse()
                    {
                        Id = user.Id,
                        Image = user.Image,
                        Token = new JwtSecurityTokenHandler().WriteToken(token),
                        LastName = user.LastName,
                        FirstName = user.FirstName,
                        MiddleName = user.MiddleName,
                        Role = new RoleDto()
                        {
                            Id = user.Role.Id,
                            Name = user.Role.Name
                        },
                        Email = user.Email,
                    };
                    return Ok(authAnswer);
                }

                return Unauthorized("Неправильный пароль или логин");
            }
        }

        [HttpPost("/sign-up")]
        [AllowAnonymous]
        public async Task<IActionResult> SignUp([FromBody] RegistrationRequest regRequest)
        {
            if (context.Users.Any(x => x.Email == regRequest.Email))
            {
                return BadRequest("Почта уже занята, выберите другую");
            }

            var sha256 = SHA256.Create();
            var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(regRequest.Password));
            var newUser = new User()
            {
                Email = regRequest.Email,
                Password = Encoding.UTF8.GetString(hash),
                LastName = regRequest.LastName,
                FirstName = regRequest.FirstName,
                MiddleName = regRequest.FirstName,
                RoleId = 2,
            };
            context.Users.Add(newUser);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpPatch("{userId:int}/visited-site")]
        public async Task<IActionResult> UserVisitedWebsite(int userId)
        {
            var sqlQuery =
                await context.Database.ExecuteSqlAsync(
                    $"UPDATE \"user\" SET has_been_on_website = true WHERE id = {userId};");
            return Ok(sqlQuery != 0);
        }
    }
}