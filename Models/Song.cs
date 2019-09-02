using System.Collections.Generic;

namespace Station.Models {
    public class Song {
        public long Id { get; set; }
        public string Title { get; set; }
        public int Duration { get; set; }
        public List<string> Genres { get; set; }
        public string MBID { get; set; }
        public List<Album> Albums { get; set; }
        public List<Artist> Artist { get; set; }
        public List<Upload> Uploads { get; set; }
    }
}
