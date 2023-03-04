using System;
using System.Collections.Generic;

namespace StarWars.DataTank.Domain.Models
{
    public class Category
    {
        public Guid CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Timeline> Timelines { get; set; }
    }
}
