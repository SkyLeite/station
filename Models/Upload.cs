using System;

namespace Station.Models
{
    public class Upload {
        public long Id { get; set; }
        public string Filename { get; set; }
        public Type Type { get; set; }
        public string Extension { get; set; }
        public int Size { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public enum Type {
        Image,
        Audio,
        Video
    }
}
