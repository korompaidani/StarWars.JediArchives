using StarWars.DataTank.Domain.Models;
using System;

namespace StarWars.DataTank.Application.Features.Timelines.Queries.GetTimelineList
{
    public class TimelineListDto
    {
        public Guid TimelineId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int StartYear { get; set; }
        public int EndYear { get; set; }
        public int Length => EndYear - StartYear;
        public string ImageUrl { get; set; }
        public Image Image { get; set; }        
    }
}
