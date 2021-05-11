using Demo3.DAL.Interface;
using Demo3.DAL.Repositories;
using Demo3.Data;
using Demo3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo3.DAL.Services
{
    public class UserService : Repository<User>, IUserService
    {
        public UserService(DemoDbContext db) : base(db)
        {
        }

        public IEnumerable<User> GetAllUsers()
        {
            return GetAll();
        }

        public User GetInfoUserById(string id)
        {
            return GetInfoByID(id);
        }

        public async Task<bool> AddUser(User user)
        {
            User user1 = GetInfoByID(user.Id);
            if(user1 != null)
            {
                return false;
            }
            await Insert(user);
            return true;
        }

        public async Task<bool> UpdateInfoUser(string id, User info )
        {
            User user =  GetInfoByID(id);
            if(user != null)
            {
                user.Id = info.Id;
                user.LastName = info.LastName;
                user.FirstName = info.FirstName;
                user.Email = info.Email;
                user.Password = info.Password;
                user.Gender = info.Gender;
                user.Phone = info.Phone;
                user.Address = info.Address;
                await Update(user);
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteUser(string id)
        {
            User category = GetInfoByID(id);
            if (category == null)
            {
                return false;
            }
            await Delete(category);
            return true;
        }

        public User GetInfoUserByEmail(string email)
        {
            User user = Get(x => x.Email == email);
            if(user == null)
            {
                return null;
            }
            return user;
        }

        
    }
}
