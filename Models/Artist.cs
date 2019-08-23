using System.Collections.Generic;

namespace Station.Models {
    public class Artist {
        public long Id { get; set; }
        public string Title { get; set; }
        public List<string> Tags { get; set; }
        public string MBID { get; set; }
        public List<Album> Albums { get; set; }
    }
}
