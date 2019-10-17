using System.Collections.Generic;

namespace Station.Models {
    public class Artist {
        public int Id { get; set; }
        public string Title { get; set; }
        public string[] Tags { get; set; }
        public string MBID { get; set; }
        public List<Album> Albums { get; set; }
    }
}
