using Demo3.AES;
using Demo3.DAL.Interface;
using Demo3.Data;
using Demo3.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Demo3.DAL.Services
{
    public class AuthenticateService : IAuthenticateService
    {
        private DemoDbContext _db;
        private readonly ApplicationSettings _appSettings;
        private UserService _serUser;
        private AESHelper _aesHelper;
        private EncryptVerifyService _serEncryptVerify;

        public AuthenticateService(DemoDbContext db, UserService serUser, IOptions<ApplicationSettings> appSettings, AESHelper aesHelper, EncryptVerifyService serEncryptVerify)
        {
            _db = db;
            _appSettings = appSettings.Value;
            _serUser = serUser;
            _aesHelper = aesHelper;
            _serEncryptVerify = serEncryptVerify;
        }
        public string Authenticate(string email, string password)
        {
            var pass = _aesHelper.DecryptStringAES(password);
            User user = _serUser.GetInfoUserByEmail(email);
            if (user == null)
            {
                return null;
            }
            //var resp = _serEncryptVerify.VerifyPassword(pass, user.Password);
            var resp = BCrypt.Net.BCrypt.Verify(pass, user.Password);
            if (resp == false)
            {
                return null;
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.JWT_SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("Id", user.Id),
                    new Claim("LastName", user.LastName),
                    new Claim("FirstName", user.FirstName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim("Gender", user.Gender.ToString()),
                    new Claim("Phone", user.Phone),
                    new Claim("Address", user.Address),
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256Signature)
            };
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(securityToken);
            return token;
        }
    }
}
