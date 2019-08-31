using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Station.Models;

namespace Station.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class SongsController : ControllerBase
    {
        private readonly Database _database;

        public SongsController(Database context) {
            this._database = context;
        }

        // GET api/songs
        [HttpGet]
        public async Task<ActionResult<Song[]>> Get()
        {
            var songs = await _database.GetSongsAsync();
            return songs;
        }

        // GET api/songs/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/songs
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/songs/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/songs/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
