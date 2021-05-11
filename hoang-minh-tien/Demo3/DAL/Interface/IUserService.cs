using Demo3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo3.DAL.Interface
{
    public interface IUserService
    {
        IEnumerable<User> GetAllUsers();
        public User GetInfoUserById(string id); 
        public Task<bool> UpdateInfoUser(string id, User info);
        public Task<bool> AddUser(User user);
        public Task<bool> DeleteUser(string id);
        public User GetInfoUserByEmail(string email);
    }
}
