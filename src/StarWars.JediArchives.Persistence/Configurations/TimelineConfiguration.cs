﻿namespace StarWars.JediArchives.Persistence.Configurations
{
    public class TimelineConfiguration : IEntityTypeConfiguration<Timeline>
    {
        public void Configure(EntityTypeBuilder<Timeline> builder)
        {
            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);
        }
    }
}