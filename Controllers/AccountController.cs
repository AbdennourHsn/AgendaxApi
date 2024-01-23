using System;
using System.Security.Cryptography;
using System.Text;
using AgendaxApi.Data;
using AgendaxApi.DTOs;
using AgendaxApi.Entities;
using AgendaxApi.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgendaxApi.Controllers
{
	public class AccountController : BaseApiController
	{
        private readonly DataContext _context;
        private readonly ITokenServise _tokenServise;

        public AccountController(DataContext context, ITokenServise tokenServise)
        {
            _context = context;
            _tokenServise = tokenServise;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<UserTokenDto>> Register(UserDto userDto)
        {
            using var hmac = new HMACSHA512();
            var user = new User
            {
                UserName = userDto.UserName,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Email = userDto.Email,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(userDto.Password)),
                PasswordSalt = hmac.Key,
                Adress = userDto.Adress,
                Country = userDto.Country,
                PostCode = userDto.PostCode,
                AboutMe = userDto.AboutMe,
                gender = userDto.gender
            };
            var project = new Project
            {
                Name = "Default Workspace", // Set a default name for the workspace
                color = "Default Color",     // Set a default color for the workspace
                User = user
            };
            _context.Users.Add(user);
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            return new UserTokenDto
            {
                UserName = user.UserName,
                Token = _tokenServise.CreateToke(user)
            };
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<UserTokenDto>> Login(LoginDto loginDto)
        {
            var user = await _context.Users.SingleOrDefaultAsync(user => user.Email == loginDto.Email);
            if (user == null) return Unauthorized("invalid username");
            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
            for (int i = 0; i < computeHash.Length; i++)
            {
                if (computeHash[i] != user.PasswordHash[i]) return Unauthorized("invalid password");
            }
            return new UserTokenDto
            {
                UserName = user.UserName,
                Token = _tokenServise.CreateToke(user)
            };
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound("User not found");
            }

            try
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return Ok("Account deleted successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateAccount(int id, UserDto updateUserDto)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound("User not found");
            }

            try
            {
                user.UserName = updateUserDto.UserName;
                user.FirstName = updateUserDto.FirstName;
                user.LastName = updateUserDto.LastName;
                user.Email = updateUserDto.Email;
                user.Adress = updateUserDto.Adress;
                user.Country = updateUserDto.Country;
                user.AboutMe = updateUserDto.AboutMe;
                user.PostCode = updateUserDto.PostCode;
                user.gender= updateUserDto.gender;
                await _context.SaveChangesAsync();
                return Ok("Account updated successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpGet("user/{id}")]
        public async Task<ActionResult<UserDto>> GetUser(int id)
        {
            User user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound("User not found");
            }
            UserDto userDto= new UserDto{
                UserName=user.UserName,
                FirstName=user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                AboutMe = user.AboutMe,
                Country = user.Country,
                Adress= user.Adress,
                gender=user.gender
            };
            return userDto;
        }
    }
}

