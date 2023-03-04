using System;

namespace StarWars.DataTank.Domain.Models
{
    public class Timeline
    {
        public Guid TimelineId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int BeginYear { get; set; }
        public int EndYear { get; set; }
        public virtual Image Image { get; set; }
        public Guid? CategoryId { get; set; }
        public virtual Category Category { get; set; }
    }
}
