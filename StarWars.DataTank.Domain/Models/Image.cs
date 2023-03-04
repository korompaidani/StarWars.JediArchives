using System;

namespace StarWars.DataTank.Domain.Models
{
    public class Image
    {
        public Guid ImageId { get; set; }
        public byte[] ThumbnailData { get; set; }
        public byte[] ImageData { get; set; }
        public virtual Timeline Timeline { get; set; }
    }
}
