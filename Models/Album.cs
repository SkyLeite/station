using System.Collections.Generic;

namespace Station.Models {
    public class Album {
        public long Id { get; set; }
        public string Title { get; set; }
        public string CoverArt { get; set; }
        public string MBID { get; set; }
        public string Type { get; set; }
        public List<string> Tags { get; set; }
        public List<Song> Songs { get; set; }
    }
}
