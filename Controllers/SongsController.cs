using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Station.Models;

namespace Station.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class SongsController : ControllerBase
    {
        private readonly Database _database;
        private readonly IEnumerable<IBasePlugin> _plugins;
        private readonly SongWorker _songWorker;

        public SongsController(Database database, IEnumerable<IBasePlugin> plugins, SongWorker songWorker) {
            this._database = database;
            this._plugins = plugins;
            this._songWorker = songWorker;
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
        [AllowAnonymous]
        [HttpPost]
        public async Task<string> Post([FromBody] PostSongBody body)
        {
            var plugin = _plugins.First(i => i.Name == body.Plugin);
            var pluginResponse = await plugin.ImportFileAsync(body.Fields);

            var song = await _songWorker.ProcessTrackAsync(pluginResponse);

            return song;
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

    public class PostSongBody {
        public string Plugin;
        public Dictionary<string, dynamic> Fields;
    }
}
