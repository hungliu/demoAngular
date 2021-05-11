using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo3.DAL.Interface
{
    public interface IEncryptVerifyService
    {
        public string EncryptPassword(string password);
        public bool VerifyPassword(string enteredPassword, string storedPassword);
    }
}
