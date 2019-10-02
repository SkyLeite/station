using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Station.Models;
using Station.Services;

namespace Station.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly Database _database;
        private readonly IUserService _userService;

        public UsersController(Database context, IUserService userService) {
            this._database = context;
            this._userService = userService;
        }

        // GET api/users
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<User[]>> Get()
        {
            var users = await _database.GetUsersAsync();
            return new JsonResult(users, new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore
            });
        }

        // GET api/users/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> Get(int id)
        {
            var user = await _database.GetUserAsync(id);
            return new JsonResult(user, new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore
            });
        }

        // POST api/users/authenticate
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<ActionResult<User>> Authenticate([FromBody] ILoginParameters loginParams) {
            Console.WriteLine(123123);
            var user = await _userService.Authenticate(loginParams.email, loginParams.password);

            if (user == null)
                return Unauthorized(new { message = "Username or password is incorrect" });

            return Ok(user);
        }

        // POST api/users
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<User>> Post([FromBody] ICreateUserParameters parameters)
        {
            var user = await _userService.CreateUser(parameters.email, parameters.password, parameters.displayName);
            return new JsonResult(user, new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore
            });
        }

        // PUT api/users/5
        [Authorize]
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/users/5
        [Authorize]
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }

    public class ICreateUserParameters {
        public string email;
        public string password;
        public string displayName;
    }

    public class ILoginParameters {
        public string email;
        public string password;
    }
}
