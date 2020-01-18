using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Zakaz25WebApi.Models;
using Zakaz25WebApi.Share;

namespace Zakaz25WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private AuthAction _auth;
        public AuthController(AuthAction auth)
        {
            _auth = auth;
        }
        [HttpPost("createuser")]
        public async Task<bool> CreateUser(User user)
        {
            var result = await _auth.RegistrationUser(user);
            return result;
        }
        [HttpPost("checkuser")]
        public async Task<bool> UserExist(User user)
        {
            var result = await _auth.IsExistUser(user);
            return result;
        }
    }
}