using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Station.Models;

namespace Station.Services {
    public interface IUserService {
        Task<User> Authenticate(string username, string password);
        Task<User> CreateUser(string email, string password, string displayName);
    }

    public class UserService : IUserService {
        public Database _database;

        public UserService(Database database) {
            _database = database;
        }

        public async Task<User> Authenticate(string email, string password) {
            var user = await _database.GetUserByEmail(email);

            if (user == null) {
                return null;
            }

            bool isPasswordValid = await Task.Run(() => BCrypt.Net.BCrypt.Verify(password, user.Password));

            if (!isPasswordValid) {
                throw new InvalidPasswordException();
            }

            user.Password = null;
            return user;
        }

        public async Task<User> CreateUser(string email, string password, string displayName) {
            var hashedPassword = await Task.Run(() => BCrypt.Net.BCrypt.HashPassword(password));
            var user = await _database.CreateUserAsync(email, hashedPassword, displayName);

            user.Password = null;
            return user;
        }
    }

    public class InvalidPasswordException : System.Exception {
        public InvalidPasswordException() : base() {  }
        public InvalidPasswordException(string message) : base(message) {  }
        public InvalidPasswordException(string message, System.Exception inner) : base(message, inner) {  }

        protected InvalidPasswordException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
