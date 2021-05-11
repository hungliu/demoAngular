using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Demo3.Data;
using Demo3.Models;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Demo3.DAL.Services;
using Demo3.Middleware;
using Demo3.AES;
using Microsoft.AspNetCore.JsonPatch;

namespace Demo3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[JwtAuthAtrribute]
    public class UsersController : ControllerBase
    {
        private UserService _serUser;
        private AuthenticateService _serAuth;
        private AESHelper _aesHelper;
        private EncryptVerifyService _serEncryptVerify;
        public UsersController(UserService userService, AuthenticateService authService, AESHelper aesHelper, EncryptVerifyService serEncryptVerify)
        {
            _serUser = userService;
            _serAuth = authService;
            _aesHelper = aesHelper;
            _serEncryptVerify = serEncryptVerify;
        }

        [HttpGet]
        public IEnumerable<User> GetAllUsers()
        {
            return _serUser.GetAllUsers();
        }

        [HttpGet("{id}")]
        public ActionResult<User> GetUser(string id)
        {
            var user = _serUser.GetInfoUserById(id);
            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        [HttpPost]
        public async Task<ActionResult> CreateUser(User user)
        {
            try
            {
                user.Password = _aesHelper.DecryptStringAES(user.Password);
                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                //user.Password = _serEncryptVerify.EncryptPassword(user.Password);
                var find = await _serUser.AddUser(user);
                if (find == false)
                {
                    return Conflict("Email already exists");
                }
                else
                {
                    return CreatedAtAction("GetUser", new { id = user.Id }, user);
                }

            }
            catch (DbUpdateException)
            {
                return StatusCode(500);
            }
        }

        [JwtAuthAtrribute]
        [HttpPatch("{id}")]
        public async Task<IActionResult> EditUser(string id, [FromBody] JsonPatchDocument<User> user)
        {
            try
            {
                if (user == null)
                {
                    return BadRequest();
                }
                var userOld = _serUser.GetInfoByID(id);
                user.ApplyTo(userOld);
                var resp = await _serUser.UpdateInfoUser(id, userOld);
                if (resp == true)
                {
                    return NoContent();
                }
                else
                {
                    return Unauthorized("Edit fail");
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                //if (!UserExists(id))
                //{
                //    return NotFound();
                //}
                //else
                //{
                //    throw;
                //}
                return StatusCode(500);
            }

        }

        [HttpPut("ChangePassword/{id}")]
        public async Task<IActionResult> ChangePassword(string id, ChangePassword change)
        {
            change.currentPassword = _aesHelper.DecryptStringAES(change.currentPassword);
            change.newPassword = _aesHelper.DecryptStringAES(change.newPassword);
            User user = _serUser.GetInfoUserById(id);
            var check = BCrypt.Net.BCrypt.Verify(change.currentPassword, user.Password);
            if (check == false)
            {
                return BadRequest();
            }
            user.Password = BCrypt.Net.BCrypt.HashPassword(change.newPassword);
            var resp = await _serUser.UpdateInfoUser(id, user);
            if (resp == true)
            {
                return NoContent();
            }
            else
            {
                return Unauthorized("Edit Password fail");
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var resp = await _serUser.DeleteUser(id);
            if(resp == true)
            {
                return NoContent();
            }
            return NotFound();
        }

        //[HttpPost]
        //[Route("Login")]
        ////POST : /api/ApplicationUser/Login
        //public async Task<IActionResult> Login(UserLogin user)
        //{
        //    User findUser = _context.Users.SingleOrDefault(u => (u.Email == user.Email && u.Password == user.Password));
        //    if (user != null && findUser != null)
        //    {
        //        var tokenDescriptor = new SecurityTokenDescriptor
        //        {
        //            Subject = new ClaimsIdentity(new Claim[]
        //            {
        //                new Claim("Id", findUser.Id),
        //                new Claim("LastName",findUser.LastName),
        //                new Claim("FirstName",findUser.FirstName),
        //                new Claim(ClaimTypes.Email,findUser.Email),
        //                new Claim("Gender",findUser.Gender.ToString()),
        //                new Claim("Phone",findUser.Phone),
        //                new Claim("Address",findUser.Address),
        //            }),
        //            Expires = DateTime.UtcNow.AddDays(7),
        //            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes("HoangMinhTien 123123123123")), SecurityAlgorithms.HmacSha256Signature)
        //        };
        //        var tokenHandler = new JwtSecurityTokenHandler();
        //        var securityToken = tokenHandler.CreateToken(tokenDescriptor);
        //        var token = tokenHandler.WriteToken(securityToken);

        //        var responeUser = new
        //        {
        //            lastName = findUser.LastName,
        //            firstName = findUser.FirstName,
        //        };

        //        return Ok(new { responeUser, token });
        //    }
        //    else
        //        return BadRequest(new { message = "Username or password is incorrect." });
        //}

        [HttpPost]
        [Route("Login")]
        public ActionResult Login(UserLogin user)
        {
            var token = _serAuth.Authenticate(user.Email,user.Password);
            if(token == null)
            {
                return BadRequest(new { message = "Username or password is incorrect. !!!" });
            }
            return Ok(new { token });
            //var pass = _aesHelper.DecryptStringAES(user.Password);
            //User find = _serUser.GetInfoUserByEmail(user.Email);
            //if(find == null)
            //{
            //    return BadRequest(new { message = "Email does not exist !!!" });
            //}
            //else
            //{
            //    var resp = BCrypt.Net.BCrypt.Verify(pass, find.Password);
            //    if (resp == false)
            //    {
            //        return Unauthorized(new { message = "Please check password !!!" });
            //    }
            //    var token = _serAuth.Authenticate(find);
            //    return Ok(new { token });
            //}
        }

        [HttpGet]
        [Route("Profile")]
        [JwtAuthAtrribute]
        public IActionResult Profile()
        {
            User user = (User)HttpContext.Items["User"];
            if(user == null)
            {
                return BadRequest();
            }
            return Ok(user);
        }
    }
}
