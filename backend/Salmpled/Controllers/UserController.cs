using FirebaseAdmin;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Salmpled.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;
using Salmpled.Models.DTOS;
using Salmpled.Services;
namespace Salmpled.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly SalmpledContext _context;

        private readonly IUserService _userService;
        public UserController(SalmpledContext context, IUserService userService)
        {
            _context = context;

            _userService = userService;

        }

        [HttpGet("dashboard")]
        [Authorize]
        public async Task<IActionResult> GetDashboard() {
            return Ok(await _userService.GetUserAuthorized());
        }

        [HttpPost]
        [Authorize]

        public async Task<IActionResult> UpdateProfileInfo([FromForm] UpdateUserDTO newData) {
            return Ok(await _userService.UpdateUserInfo(newData));
        }

        [HttpPost("createFromGoogle")]
        [Authorize]
        public async Task<ActionResult<User>> CreateUserFromGoogle()
        {

            try
            {
                String header = this.HttpContext.Request.Headers["Authorization"];
                var auth = FirebaseAdmin.Auth.FirebaseAuth.DefaultInstance;

                var decoded = await auth.VerifyIdTokenAsync(header.Split(" ")[1]);
                var uid = decoded.Uid;
                var user = await _context.User.FindAsync(uid);
                if (user != null)
                {
                    return Ok(user);
                }
                else
                {
                    var fuser = await auth.GetUserAsync(uid);
                    var newuser = new User
                    {
                        Id = uid,
                        AuthProvider = fuser.ProviderId,
                        Email = fuser.Email,
                    };
                    _context.User.Add(newuser);
                    await _context.SaveChangesAsync();
                    return Ok(newuser);
                }
            }
            catch (FirebaseException ex)
            {
                Console.WriteLine(ex);
                return BadRequest();
            }

        }

        [HttpGet("{username}/avail")]
        [Authorize]
        public async Task<ActionResult<bool>> UsernameAvailable(string username)
        {

            User user = await _context.User.Where(u => u.Username == username).FirstOrDefaultAsync();

            if (user == null)
            {
                return true;
            }

            return false;
        }

        [HttpPost("setusername")]
        [Authorize]
        public async Task<IActionResult> SetUsername(SetUsernameDTO data)
        {
            return Ok(await _userService.SetUsername(data));   
        }

        [HttpGet("{username}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUserByUsername(string username)
        {
            return Ok(await _userService.GetUserByUsername(username));
        }

    }
}


