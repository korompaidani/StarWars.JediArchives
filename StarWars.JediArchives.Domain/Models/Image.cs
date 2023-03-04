using System;

namespace StarWars.JediArchives.Domain.Models
{
    public class Image
    {
        public Guid ImageId { get; set; }
        public string OriginalFileName { get; set; }
        public byte[] ThumbnailData { get; set; }
        public byte[] ImageData { get; set; }
        public Guid TimelineId { get; set; }
        public virtual Timeline Timeline { get; set; }
    }
}