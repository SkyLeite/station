using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Station.Models;

namespace Station.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class LibrariesController : ControllerBase
    {
        private readonly Database _database;

        public LibrariesController(Database context) {
            this._database = context;
        }

        // GET api/libraries
        [HttpGet]
        public async Task<ActionResult<Library[]>> Get()
        {
            var libraries = await _database.GetLibrariesAsync();
            return libraries;
        }

        // GET api/libraries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Library>> Get(int id)
        {
            var library = await _database.GetLibraryAsync(id);
            return library;
        }

        // POST api/libraries
        [HttpPost]
        public async Task<ActionResult<Library>> Post([FromBody] ICreateLibraryParameters parameters)
        {
            var library = await _database.CreateLibraryAsync(parameters.name, parameters.ownerid);
            return library;
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

    public class ICreateLibraryParameters {
        public string name;
        public int ownerid;
    }
}
