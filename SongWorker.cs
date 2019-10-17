using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Station.Models;
using System.Security.Cryptography;
using System;
using System.IO;
using Type = Station.Models.Type;
using MusicBrainz.Data;
using System.Collections.Generic;

namespace Station
{
    public class SongWorker {
        private readonly IOptions<ApplicationSettings> _settings;
        private readonly Database _database;

        public SongWorker(IOptions<ApplicationSettings> settings, Database database) {
            _settings = settings;
            _database = database;
        }

        public string CalculateMD5(byte[] bytes)
        {
            using (var sha = new SHA256Managed())
            {
                using (var stream = new MemoryStream(bytes))
                {
                    var hash = sha.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", String.Empty);
                }
            }
        }

        private async Task<List<RecordingData>> GetMusicbrainzInfo(string track, string artist, string album = null) {
            var response = await Task.Run(() => MusicBrainz.Search.Recording(recording: track, artist: artist, release: album));
            return response.Data;
        }

        private async Task<List<RecordingData>> GetMusicbrainzInfo(string mbid) {
            var response = await Task.Run(() => MusicBrainz.Search.Recording(reid: mbid));
            return response.Data;
        }

        public async Task<Song> ProcessTrackAsync(PluginResponse pluginResponse) {
            var hash = this.CalculateMD5(pluginResponse.Data);
            var fileDestination = Path.Combine(_settings.Value.MusicDirectory, $"{hash}.mp3");
            await File.WriteAllBytesAsync(fileDestination, pluginResponse.Data);

            var upload = await _database.CreateUploadAsync(fileDestination, "mp3", Type.Audio, pluginResponse.Data.Length);

            List<RecordingData> metadata;
            if (pluginResponse.MBID == null) {
                metadata = await this.GetMusicbrainzInfo(pluginResponse.Title, pluginResponse.Artist, pluginResponse.Album);
            } else {
                var results = await this.GetMusicbrainzInfo(pluginResponse.MBID);
                if (results.Count == 0) {
                    metadata = await this.GetMusicbrainzInfo(pluginResponse.Title, pluginResponse.Artist, pluginResponse.Album);
                } else {
                    metadata = results;
                }
            }

            var recording = metadata[0];
            List<int> artists = new List<int>();
            foreach (var artistCredit in recording.Artistcredit) {
                var artist = await _database.CreateArtistAsync(artistCredit.Artist.Name, new List<string> { "tag1", "tag2" }, artistCredit.Artist.Id);
                artists.Add(artist.Id);
            }

            var release = recording.Releaselist[0];
            List<int> albumArtists = new List<int>();

            if (albumArtists.Count > 0) {
                foreach (var artistCredit in release.Artistcredit) {
                    var artist = await _database.CreateArtistAsync(artistCredit.Artist.Name, null, artistCredit.Artist.Id);
                    artists.Add(artist.Id);
                }
            } else {
                albumArtists.Add(artists[0]);
            }

            var album = await _database.CreateAlbumAsync(release.Title, albumArtists[0], "album", mbid: release.Id);
            var song = await _database.CreateSongAsync(recording.Title, recording.Length, null, album.Id, artists, 1, recording.Id);

            return song;
        }
    }
}
