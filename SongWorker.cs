using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Station.Models;
using System.Security.Cryptography;
using System;
using System.IO;

namespace Station
{
    public class SongWorker {
        private readonly IOptions<ApplicationSettings> _settings;

        public SongWorker(IOptions<ApplicationSettings> settings) {
            _settings = settings;
        }

        public string CalculateMD5(byte[] bytes)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = new MemoryStream(bytes))
                {
                    var hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }

        public async Task<string> ProcessTrackAsync(PluginResponse pluginResponse) {
            var hash = this.CalculateMD5(pluginResponse.Data);
            var fileDestination = Path.Combine(_settings.Value.MusicDirectory, $"{hash}.mp3");

            await File.WriteAllBytesAsync(fileDestination, pluginResponse.Data);

            return "hello!";
        }
    }
}
