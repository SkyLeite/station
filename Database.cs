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
    }

    public class DatabaseOptions
    {
        public readonly string ConnectionString;
    }
}
