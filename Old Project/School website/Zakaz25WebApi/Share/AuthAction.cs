using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zakaz25WebApi.Context;
using Zakaz25WebApi.Models;

namespace Zakaz25WebApi.Share
{
    public class AuthAction
    {
        private readonly DataContext _context;
        public AuthAction(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> RegistrationUser(User user)
        {
            if(user.Password != null && user.Password != null)
            {
                await _context._User.InsertOneAsync(new User
                {
                    Login = user.Login,
                    Password = user.Password
                });
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<bool> IsExistUser(User user)//nado neskolko sovpadenii сколько? emaill and pass
        {
            var res = await _context._User.Find(u => u.Login == user.Login && u.Password == user.Password).AnyAsync();
            return res;
        }
    }
}
