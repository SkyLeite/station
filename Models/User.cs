using System.Collections.Generic;

namespace Station.Models {
    public class User {
        public long Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string DisplayName { get; set; }
        public List<Library> Libraries { get; set; }
    }
}
