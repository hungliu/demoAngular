using Demo3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo3.DAL.Interface
{
    public interface IAuthenticateService
    {
        public string Authenticate(string email,string password);

        
    }
}
