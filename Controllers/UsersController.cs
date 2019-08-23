using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Station.Models;

namespace Station.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly Database _database;

        public UsersController(Database context) {
            this._database = context;
        }

        // GET api/users
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
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> Get(int id)
        {
            var user = await _database.GetUserAsync(id);
            return new JsonResult(user, new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore
            });
        }

        // POST api/users
        [HttpPost]
        public async Task<ActionResult<User>> Post([FromBody] ICreateUserParameters parameters)
        {
            var user = await _database.CreateUserAsync(parameters.email, parameters.password, parameters.displayName);
            return new JsonResult(user, new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore
            });
        }

        // PUT api/users/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/users/5
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
}
