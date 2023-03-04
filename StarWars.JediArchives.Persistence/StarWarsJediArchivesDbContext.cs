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

            modelBuilder.Entity<Image>().HasData(new Image
            {
                ImageId = Guid.NewGuid(),
                ImageData = new byte[] { 0, 1, 2 },
                OriginalFileName = "",
                ThumbnailData = new byte[] { 0 }
            });

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
                Description = "Mace Windu was a revered human male Jedi Master and member of the Jedi High Council during the last years of the Galactic Republic. During his time in the Jedi Order, he once served as elected leader of the Jedi and, during the Clone Wars, as a Jedi General in the Grand Army of the Republic. He was the greatest champion of the Jedi Order and promoted its ancient traditions amidst the growing influence of the dark side of the Force in the corrupt, declining days of the Republic.",
                StartYear = -72,
                EndYear = -19,
                CategoryId = characterGuid,
                ImageUrl = "https://static.wikia.nocookie.net/starwars/images/7/7e/MaceWindu.jpg/revision/latest?cb=20180813112704&path-prefix=hu",
            });
            modelBuilder.Entity<Timeline>().HasData(new Timeline
            {
                TimelineId = Guid.NewGuid(),
                Name = "Qui-Gon Jinn",
                Description = "Qui-Gon Jinn, a Force-sensitive human male, was a venerable if maverick Jedi Master who lived during the last years of the Republic Era. He was a wise and well-respected member of the Jedi Order, and was offered a seat on the Jedi Council, but chose to reject and follow his own path. Adhering to a philosophy centered around the Living Force, Jinn strove to follow the will of the Force even when his actions conflicted with the wishes of the High Council. After encountering Anakin Skywalker, Jinn brought him to the Jedi Temple on Coruscant, convinced he had found the Chosen One. His dying wish was for Skywalker to become a Jedi and ultimately restore balance to the Force.",
                StartYear = -80,
                EndYear = -32,
                CategoryId = characterGuid,
                ImageUrl = "https://upload.wikimedia.org/wikipedia/en/a/ad/Qui-Gon_Jinn.png",
            });
            modelBuilder.Entity<Timeline>().HasData(new Timeline
            {
                TimelineId = Guid.NewGuid(),
                Name = "Anakin Skywalker",
                Description = "Anakin was a human male Jedi Knight of the Galactic Republic and the prophesied Chosen One of the Jedi Order, destined to bring balance to the Force. Also known as Ani during his childhood, Skywalker earned the moniker Hero With No Fear from his accomplishments in the Clone Wars. His alter ego, Darth Vader, the Dark Lord of the Sith, was created when Skywalker turned to the dark side of the Force, pledging his allegiance to the Sith Lord Darth Sidious at the end of the Republic Era.",
                StartYear = -41,
                EndYear = 4,
                CategoryId = characterGuid,
                ImageUrl = "https://static.wikia.nocookie.net/starwars/images/6/6f/Anakin_Skywalker_RotS.png/revision/latest?cb=20160427145922&path-prefix=hu",
            });
            modelBuilder.Entity<Timeline>().HasData(new Timeline
            {
                TimelineId = Guid.NewGuid(),
                Name = "Obi-Wan Kenobi",
                Description = "Obi-Wan Kenobi was a legendary human male Jedi Master who served on the Jedi High Council during the last years of the Republic Era. During the Imperial Era, he adopted the alias Ben Kenobi in order to hide from the regime that drove the Jedi to near extinction. A noble man known for his skills with the Force, Kenobi trained Anakin Skywalker, served as a Jedi General in the Grand Army of the Republic, and became a mentor to Luke Skywalker prior to his death in 0 BBY.",
                StartYear = -57,
                EndYear = 0,
                CategoryId = characterGuid,
                ImageUrl = "https://www.giantfreakinrobot.com/wp-content/uploads/2022/05/obi-wan-kenobi-900x506.jpg",
            });
            modelBuilder.Entity<Timeline>().HasData(new Timeline
            {
                TimelineId = Guid.NewGuid(),
                Name = "Count Dooku",
                Description = "Dooku, a Force-sensitive human male, was a Jedi Master that fell to the dark side of the Force and became the Dark Lord of the Sith Darth Tyranus during the final years of the Galactic Republic. After leaving the Jedi Order, he claimed the title Count of Serenno and, during the Clone Wars, served as Head of State of the Confederacy of Independent Systems. He was the second apprentice of Darth Sidious, the Dark Lord of the Sith whose plan to conquer the galaxy relied on Dooku leading a pan-galactic secessionist movement against the Republic. As such, Dooku immersed himself in the dark side and worked tirelessly to advance his and his master's plans, but ultimately forgot that treachery was the way of the Sith.",
                StartYear = -102,
                EndYear = -19,
                CategoryId = characterGuid,
                ImageUrl = "",
            });
            modelBuilder.Entity<Timeline>().HasData(new Timeline
            {
                TimelineId = Guid.NewGuid(),
                Name = "Sheev Palpatine",
                Description = "Darth Sidious, born Sheev Palpatine and known simply as the Emperor, was a human male Dark Lord of the Sith who reigned over the galaxy as Galactic Emperor of the First Galactic Empire. Rising to power in the Galactic Senate as the senator of Naboo, the manipulative Sith Lord cultivated two identities, Sidious and Palpatine, using both to further his political career. He masterminded the Clone Wars in order to gain dictatorial powers during the final years of the Galactic Republic. After the fall of the Jedi Order, Sidious established a reign lasting from 19 BBY until his demise in 4 ABY. Death was not the end for Sidious, however, as the dark side of the Force was a pathway to abilities that made his return possible.",
                StartYear = -84,
                EndYear = 4,
                CategoryId = characterGuid,
                ImageUrl = "",
            });
            modelBuilder.Entity<Timeline>().HasData(new Timeline
            {
                TimelineId = Guid.NewGuid(),
                Name = "Luke Skylwalker",
                Description = "Darth Sidious, born Sheev Palpatine and known simply as the Emperor, was a human male Dark Lord of the Sith who reigned over the galaxy as Galactic Emperor of the First Galactic Empire. Rising to power in the Galactic Senate as the senator of Naboo, the manipulative Sith Lord cultivated two identities, Sidious and Palpatine, using both to further his political career. He masterminded the Clone Wars in order to gain dictatorial powers during the final years of the Galactic Republic. After the fall of the Jedi Order, Sidious established a reign lasting from 19 BBY until his demise in 4 ABY. Death was not the end for Sidious, however, as the dark side of the Force was a pathway to abilities that made his return possible.",
                StartYear = -19,
                EndYear = 35,
                CategoryId = characterGuid,
                ImageUrl = "",
            });
            modelBuilder.Entity<Timeline>().HasData(new Timeline
            {
                TimelineId = Guid.NewGuid(),
                Name = "Han Solo",
                Description = "Han Solo rose from an impoverished childhood on the mean streets of Corellia to become one of the heroes of the Rebel Alliance. As captain of the Millennium Falcon, Han and his co-pilot Chewbacca came to believe in the cause of galactic freedom, joining Luke Skywalker and Princess Leia Organa in the fight against the Empire. After the Battle of Endor, Han faced difficult times in a chaotic galaxy, leading to a shattering confrontation with his estranged son Ben.",
                StartYear = -32,
                EndYear = 34,
                CategoryId = characterGuid,
                ImageUrl = "",
            });
            modelBuilder.Entity<Timeline>().HasData(new Timeline
            {
                TimelineId = Guid.NewGuid(),
                Name = "Padme Amidala",
                Description = "Padmé Amidala, also known as Padmé Amidala Naberrie, was a human female senator who represented the people of Naboo during the final years of the Galactic Republic. Prior to her career in the Galactic Senate, Amidala was the elected ruler of the Royal House of Naboo. A political idealist, she advocated for the preservation of democracy as well as a peaceful resolution to the Clone Wars. However, her secret marriage to the Jedi Knight Anakin Skywalker would have a lasting effect on the future of the galaxy for decades to come.",
                StartYear = -46,
                EndYear = 19,
                CategoryId = characterGuid,
                ImageUrl = "",
            });
            modelBuilder.Entity<Timeline>().HasData(new Timeline
            {
                TimelineId = Guid.NewGuid(),
                Name = "Jango Fett",
                Description = "Jango Fett was a human male Mandalorian bounty hunter and the clone template of the Grand Army of the Republic. Known as the best bounty hunter in the galaxy during the final years of the Galactic Republic, Fett was proficient in marksmanship as well as unarmed combat. The Mandalorian armor that he wore featured various weapons and gadgets, including a flamethrower, dual WESTAR-34 blaster pistols, and a jetpack. His personal starship was the Firespray-31-class patrol and attack craft Slave I.",
                StartYear = -66,
                EndYear = -22,
                CategoryId = characterGuid,
                ImageUrl = "",
            });
            modelBuilder.Entity<Timeline>().HasData(new Timeline
            {
                TimelineId = Guid.NewGuid(),
                Name = "Jabba the Hutt",
                Description = "Jabba Desilijic Tiure, more commonly referred to as Jabba the Hutt or simply Jabba, and formally styled as His Excellency Jabba Desilijic Tiure of Nal Hutta, Eminence of Tatooine, was a Hutt gangster and crime lord, as well as a member of the Grand Hutt Council, who operated and led a criminal empire from his palace on the Outer Rim world of Tatooine. Jabba was a major figure on Tatooine, where he controlled the bulk of the trafficking in illegal goods, piracy and slavery that generated most of the planet's wealth. He was also highly influential in the entire Outer Rim as one of its most powerful crime lords.",
                StartYear = -600,
                EndYear = 4,
                CategoryId = characterGuid,
                ImageUrl = "",
            });
            modelBuilder.Entity<Timeline>().HasData(new Timeline
            {
                TimelineId = Guid.NewGuid(),
                Name = "Ben Solo",
                Description = "Ben Solo was a Force-sensitive human male Jedi who returned to the light side of the Force by renouncing the dark side. His alter ego, Kylo Ren, was the master of the Knights of Ren and Supreme Leader of the First Order. A product of Jedi and Sith teachings, Ren embodied the conflict between the dark side and the light, making the young Force warrior dangerously unstable. Yet it was through discord that he derived power, and he learned to channel his anger into strength. Inspired by the legacy of his grandfather, the Sith Lord Darth Vader, Ren sought to destroy the last remnants of the Jedi Order and conquer the galaxy.",
                StartYear = 5,
                EndYear = 35,
                CategoryId = characterGuid,
                ImageUrl = "",
            });
            modelBuilder.Entity<Timeline>().HasData(new Timeline
            {
                TimelineId = Guid.NewGuid(),
                Name = "Shaak Ti",
                Description = "Shaak Ti was a Togruta female Jedi Master of the Jedi Order and member of the Jedi High Council in the waning years of the Galactic Republic. Considered to be amongst the wisest and most patient in the Order, she was a skilled fighter and devoted believer in the ways of the Jedi. During the Clone Wars, a galaxy-wide conflict between the Republic and the Confederacy of Independent Systems, Ti took part in the pivotal battles of Geonosis, Kamino, and Coruscant as a general for the Grand Army of the Republic.",
                StartYear = -59,
                EndYear = 3,
                CategoryId = characterGuid,
                ImageUrl = "",
            });
            modelBuilder.Entity<Timeline>().HasData(new Timeline
            {
                TimelineId = Guid.NewGuid(),
                Name = "Ki-Adi Mundi",
                Description = "Ki-Adi-Mundi, a Force-sensitive Cerean male, was a Jedi Master and member of the Jedi High Council during the last years of the Galactic Republic. By the time of the Clone Wars, Mundi became a Jedi General of the Grand Army of the Republic. Like his Jedi colleagues, he led the Republic clone troopers against the Separatist Alliance forces in several battles across the galaxy, including the first and second campaign on Geonosis and the Outer Rim Sieges. In 19 BBY, the third and final year of the war, Mundi oversaw the Republic invasion of Mygeeto with the 21st Nova Corps under his command. During the campaign, Supreme Chancellor Sheev Palpatine instructed the Grand Army soldiers to execute their Jedi leaders in accordance with Order 66, an act which resulted in the death of Mundi along with the majority of the Jedi Order.",
                StartYear = -92,
                EndYear = -19,
                CategoryId = characterGuid,
                ImageUrl = "",
            });
            modelBuilder.Entity<Timeline>().HasData(new Timeline
            {
                TimelineId = Guid.NewGuid(),
                Name = "Jedi Fallen Order",
                Description = "",
                StartYear = -19,
                EndYear = -14,
                CategoryId = mediaGuid,
                ImageUrl = "",
            });
            modelBuilder.Entity<Timeline>().HasData(new Timeline
            {
                TimelineId = Guid.NewGuid(),
                Name = "First Hyperdrive by Devaronians",
                Description = "",
                StartYear = -30000,
                EndYear = -30000,
                CategoryId = eventGuid,
                ImageUrl = "",
            });
            modelBuilder.Entity<Timeline>().HasData(new Timeline
            {
                TimelineId = Guid.NewGuid(),
                Name = "Republic Era",
                Description = "",
                StartYear = -1000,
                EndYear = -19,
                CategoryId = eraGuid,
                ImageUrl = "",
            });
        }
    }
}
