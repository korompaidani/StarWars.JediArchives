using Microsoft.EntityFrameworkCore;
using StarWars.JediArchives.Domain.Models;
using System;

namespace StarWars.JediArchives.Persistence
{
    public class StarWarsJediArchivesDbContext : DbContext
    {
        public StarWarsJediArchivesDbContext(DbContextOptions<StarWarsJediArchivesDbContext> options)
           : base(options)
        {
        }

        public DbSet<Timeline> TimeLines { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Image> Images { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(StarWarsJediArchivesDbContext).Assembly);

            var characterGuid = Guid.Parse("{3798efc7-4e33-40f6-b160-bcea5ff182d0}");
            var eraGuid = Guid.Parse("{9c527a6e-a289-49a1-a1ad-6c7445a7c928}");
            var eventGuid = Guid.Parse("{9a12c346-cf77-466d-88ce-73ea07e83f54}");
            var mediaGuid = Guid.Parse("{ceda4792-392a-4920-a5b5-9b6abe04fca5}");

            modelBuilder.Entity<Category>().HasData(new Category
            {
                CategoryId = characterGuid,
                Name = "Character",
                Description = "Any living or artificial character from Star Wars Universe"
            });
            modelBuilder.Entity<Category>().HasData(new Category
            {
                CategoryId = eraGuid,
                Name = "Era",
                Description = "Well-known ages and eras"
            });
            modelBuilder.Entity<Category>().HasData(new Category
            {
                CategoryId = eventGuid,
                Name = "Event",
                Description = "Historical or Natural events, happenings"
            });
            modelBuilder.Entity<Category>().HasData(new Category
            {
                CategoryId = mediaGuid,
                Name = "Media",
                Description = "Any Canon Film, Serie, Comic, Book, Cartoon from Star Wars Universe"
            });

            modelBuilder.Entity<Timeline>().HasData(new Timeline
            {
                TimelineId = Guid.NewGuid(),
                Name = "Yoda",
                Description = "A Force-sensitive male being belonging to a mysterious species, was a legendary Jedi Master who witnessed the rise and fall of the Galactic Republic, followed by the rise of the Galactic Empire. Small in stature but revered for his wisdom and power, Yoda trained generations of Jedi, ultimately serving as the Grand Master of the Jedi Order. Having lived through nine centuries of galactic history, he played integral roles in the Clone Wars, the rebirth of the Jedi through Luke Skywalker, and unlocking the path to immortality.",
                StartYear = -896,
                EndYear = 4,
                CategoryId = characterGuid,
                ImageUrl = "https://static.wikia.nocookie.net/starwars/images/d/d6/Yoda_SWSB.png",
            });
            modelBuilder.Entity<Timeline>().HasData(new Timeline
            {
                TimelineId = Guid.NewGuid(),
                Name = "Mace Windu",
                Description = "",
                StartYear = -72,
                EndYear = -19,
                CategoryId = characterGuid,
                ImageUrl = "",
            });
            modelBuilder.Entity<Timeline>().HasData(new Timeline
            {
                TimelineId = Guid.NewGuid(),
                Name = "",
                Description = "",
                StartYear = -72,
                EndYear = -19,
                CategoryId = characterGuid,
                ImageUrl = "",
            });
            modelBuilder.Entity<Timeline>().HasData(new Timeline
            {
                TimelineId = Guid.NewGuid(),
                Name = "",
                Description = "",
                StartYear = -72,
                EndYear = -19,
                CategoryId = characterGuid,
                ImageUrl = "",
            });
            modelBuilder.Entity<Timeline>().HasData(new Timeline
            {
                TimelineId = Guid.NewGuid(),
                Name = "",
                Description = "",
                StartYear = -72,
                EndYear = -19,
                CategoryId = characterGuid,
                ImageUrl = "",
            });
            modelBuilder.Entity<Timeline>().HasData(new Timeline
            {
                TimelineId = Guid.NewGuid(),
                Name = "",
                Description = "",
                StartYear = -72,
                EndYear = -19,
                CategoryId = characterGuid,
                ImageUrl = "",
            });
        }
    }
}
