using System.Data;
using Npgsql;
using Dapper;
using Microsoft.Extensions.Options;
using Station.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace Station
{
    public class Database {
        private readonly IOptions<ApplicationSettings> _settings;

        public Database(IOptions<ApplicationSettings> settings) {
            _settings = settings;
        }

        public async Task<User[]> GetUsersAsync() {
            using (var connection = new NpgsqlConnection(_settings.Value.ConnectionString)) {
                await connection.OpenAsync();

                var users = (await connection.QueryAsync("SELECT * FROM getusers()")).ToList();

                return users.Select(row => new User {
                        Id = row.id,
                        DisplayName = row.display_name,
                    }).ToArray();
            }
        }

        public async Task<User> GetUserAsync(long id) {
            using (var connection = new NpgsqlConnection(_settings.Value.ConnectionString)) {
                await connection.OpenAsync();

                var users = await connection.QueryAsync<User>("SELECT * FROM getuser(@userId)", new {
                        userId = id
                    });

                return users.First();
            }
        }

        public async Task<User> CreateUserAsync(string email, string password, string displayName) {
            using (var connection = new NpgsqlConnection(_settings.Value.ConnectionString)) {
                await connection.OpenAsync();

                var user = await connection.QueryAsync<User>("SELECT * FROM createuser(:email, :password, :displayName)", new {
                        email = email,
                        password = password,
                        displayName = displayName,
                    });

                return user.First();
            }
        }

        public async Task<Song[]> GetSongsAsync() {
            using (var connection = new NpgsqlConnection(_settings.Value.ConnectionString)) {
                await connection.OpenAsync();

                var rows = await connection.QueryAsync("public.GetSong", commandType: CommandType.StoredProcedure);

                return rows.Select(row => new Song {
                    Id = row.song_id,
                    Title = row.song_title,
                }).ToArray();
            }
        }

        public async Task<Library> CreateLibraryAsync(string name, int ownerid) {
            using (var connection = new NpgsqlConnection(_settings.Value.ConnectionString)) {
                await connection.OpenAsync();

                var libraries = await connection.QueryAsync<Library>("SELECT * FROM createlibrary(:name, :ownerid)", new {
                        name = name,
                        ownerid = ownerid,
                    });

                return libraries.First();
            }
        }

        public async Task<Library> GetLibraryAsync(long id) {
            using (var connection = new NpgsqlConnection(_settings.Value.ConnectionString)) {
                await connection.OpenAsync();

                var library = (await connection.QueryAsync<Library>("SELECT * FROM getlibrary(@id)", new {
                        id = id,
                        })).First();

                return library;
            }
        }

        public async Task<Library[]> GetLibrariesAsync() {
            using (var connection = new NpgsqlConnection(_settings.Value.ConnectionString)) {
                await connection.OpenAsync();

                var libraries = await connection.QueryAsync<Library>("SELECT * FROM getlibraries()");

                return libraries.ToArray();
            }
        }

        public async Task<Upload> CreateUploadAsync(string filename, string extension, Models.Type type, int size) {
            using (var connection = new NpgsqlConnection(_settings.Value.ConnectionString)) {
                await connection.OpenAsync();

                var upload = (await connection.QueryAsync<Upload>("SELECT * FROM createupload(@filename, @extension, @type, @size)", new {
                            filename = filename,
                            extension = extension,
                            type = type,
                            size = size,
                        })).First();

                return upload;
            }
        }

        public async Task<Song> CreateSongAsync(string title, int duration, IEnumerable<string> genres, int album_id, IEnumerable<int> artists, int library_id, string mbid = null) {
            using (var connection = new NpgsqlConnection(_settings.Value.ConnectionString)) {
                await connection.OpenAsync();

                var song = (await connection.QueryAsync<Song>("createsong", new {
                            title = title,
                            duration = duration,
                            genres = genres,
                            album_id = album_id,
                            artists = artists,
                            library_id = library_id,
                            mbid = mbid,
                        }, commandType: CommandType.StoredProcedure)).First();

                return song;
            }
        }

        public async Task<Artist> CreateArtistAsync(string title, IEnumerable<string> tags = null, string mbid = null) {
            using (var connection = new NpgsqlConnection(_settings.Value.ConnectionString)) {
                await connection.OpenAsync();

                var parameters = new DynamicParameters();
                parameters.Add("title", title);
                parameters.Add("tags", tags);
                parameters.Add("mbid", mbid);

                var artist = (await connection.QueryAsync<Artist>("createartist", parameters, commandType: CommandType.StoredProcedure)).First();

                return artist;
            }
        }

        public async Task<Album> CreateAlbumAsync(string title, int id_artists, string type, IEnumerable<string> tags = null, string mbid = null) {
            using (var connection = new NpgsqlConnection(_settings.Value.ConnectionString)) {
                await connection.OpenAsync();

                var album = (await connection.QueryAsync<Album>("createalbum", new {
                            title = title,
                            id_artists = id_artists,
                            type = type,
                            tags = tags,
                            mbid = mbid,
                        }, commandType: CommandType.StoredProcedure)).First();

                return album;
            }
        }
    }

    public class DatabaseOptions
    {
        public readonly string ConnectionString;
    }
}
