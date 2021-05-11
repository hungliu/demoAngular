using Demo3.AES;
using Demo3.DAL.Interface;
using Demo3.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Demo3.DAL.Services
{
    public class EncryptVerifyService : IEncryptVerifyService
    {

        private readonly ApplicationSettings _appSettings;
        private UserService _serUser;
        private AESHelper _aesHelper;

        public EncryptVerifyService(UserService serUser, IOptions<ApplicationSettings> appSettings, AESHelper aesHelper)
        {
            _appSettings = appSettings.Value;
            _serUser = serUser;
            _aesHelper = aesHelper;
        }
        public string EncryptPassword(string password)
        {
            byte[] salt = new byte[] { 204, 145, 147, 85, 151, 70, 90, 123, 99, 159, 100, 77, 122, 98, 12, 30 };
            //byte[] salt = new byte[128 / 8]; // Generate a 128-bit salt using a secure PRNG
            //using (var rng = RandomNumberGenerator.Create())
            //{
            //    rng.GetBytes(salt);
            //}
            string encryptedPassw = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8
            ));
            return encryptedPassw;
        }

        public bool VerifyPassword(string enteredPassword, string storedPassword)
        {
            byte[] salt = new byte[] { 204, 145, 147, 85, 151, 70, 90, 123, 99, 159, 100, 77, 122, 98, 12, 30 };
            string encryptedPassw = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: enteredPassword,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8
                ));
            return encryptedPassw == storedPassword;
        }
    }
}
