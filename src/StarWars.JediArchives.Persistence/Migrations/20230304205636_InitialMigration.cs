﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace StarWars.JediArchives.Persistence.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    ImageId = table.Column<Guid>(nullable: false),
                    OriginalFileName = table.Column<string>(nullable: false),
                    ThumbnailData = table.Column<byte[]>(nullable: false),
                    ImageData = table.Column<byte[]>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.ImageId);
                });

            migrationBuilder.CreateTable(
                name: "TimeLines",
                columns: table => new
                {
                    TimelineId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Description = table.Column<string>(nullable: false),
                    StartYear = table.Column<int>(nullable: false),
                    EndYear = table.Column<int>(nullable: false),
                    ImageUrl = table.Column<string>(nullable: false),
                    CategoryId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeLines", x => x.TimelineId);
                    table.ForeignKey(
                        name: "FK_TimeLines_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "Description", "Name" },
                values: new object[,]
                {
                    { new Guid("3798efc7-4e33-40f6-b160-bcea5ff182d0"), "Any living or artificial character from Star Wars Universe", "Character" },
                    { new Guid("9c527a6e-a289-49a1-a1ad-6c7445a7c928"), "Well-known ages and eras", "Era" },
                    { new Guid("9a12c346-cf77-466d-88ce-73ea07e83f54"), "Historical or Natural events, happenings", "Event" },
                    { new Guid("ceda4792-392a-4920-a5b5-9b6abe04fca5"), "Any Canon Film, Serie, Comic, Book, Cartoon from Star Wars Universe", "Media" }
                });

            migrationBuilder.InsertData(
                table: "TimeLines",
                columns: new[] { "TimelineId", "CategoryId", "Description", "EndYear", "ImageUrl", "Name", "StartYear" },
                values: new object[,]
                {
                    { new Guid("a3fdda23-ac25-4a16-8744-a82a45ba0705"), new Guid("3798efc7-4e33-40f6-b160-bcea5ff182d0"), "A Force-sensitive male being belonging to a mysterious species, was a legendary Jedi Master who witnessed the rise and fall of the Galactic Republic, followed by the rise of the Galactic Empire. Small in stature but revered for his wisdom and power, Yoda trained generations of Jedi, ultimately serving as the Grand Master of the Jedi Order. Having lived through nine centuries of galactic history, he played integral roles in the Clone Wars, the rebirth of the Jedi through Luke Skywalker, and unlocking the path to immortality.", 4, "https://static.wikia.nocookie.net/starwars/images/d/d6/Yoda_SWSB.png", "Yoda", -896 },
                    { new Guid("7bc36238-f175-4b65-8b46-448635bfb762"), new Guid("9c527a6e-a289-49a1-a1ad-6c7445a7c928"), "", -19, "", "Republic Era", -1000 },
                    { new Guid("9ca7e581-b219-449d-92f0-48b2db7fd0ce"), new Guid("3798efc7-4e33-40f6-b160-bcea5ff182d0"), "Ki-Adi-Mundi, a Force-sensitive Cerean male, was a Jedi Master and member of the Jedi High Council during the last years of the Galactic Republic. By the time of the Clone Wars, Mundi became a Jedi General of the Grand Army of the Republic. Like his Jedi colleagues, he led the Republic clone troopers against the Separatist Alliance forces in several battles across the galaxy, including the first and second campaign on Geonosis and the Outer Rim Sieges. In 19 BBY, the third and final year of the war, Mundi oversaw the Republic invasion of Mygeeto with the 21st Nova Corps under his command. During the campaign, Supreme Chancellor Sheev Palpatine instructed the Grand Army soldiers to execute their Jedi leaders in accordance with Order 66, an act which resulted in the death of Mundi along with the majority of the Jedi Order.", -19, "", "Ki-Adi Mundi", -92 },
                    { new Guid("9a9be8ad-500c-47a1-ab35-6b1f4ab3d0b1"), new Guid("3798efc7-4e33-40f6-b160-bcea5ff182d0"), "Shaak Ti was a Togruta female Jedi Master of the Jedi Order and member of the Jedi High Council in the waning years of the Galactic Republic. Considered to be amongst the wisest and most patient in the Order, she was a skilled fighter and devoted believer in the ways of the Jedi. During the Clone Wars, a galaxy-wide conflict between the Republic and the Confederacy of Independent Systems, Ti took part in the pivotal battles of Geonosis, Kamino, and Coruscant as a general for the Grand Army of the Republic.", 3, "", "Shaak Ti", -59 },
                    { new Guid("2b95b331-a58e-4d09-9f45-9bbb7ab31113"), new Guid("3798efc7-4e33-40f6-b160-bcea5ff182d0"), "Ben Solo was a Force-sensitive human male Jedi who returned to the light side of the Force by renouncing the dark side. His alter ego, Kylo Ren, was the master of the Knights of Ren and Supreme Leader of the First Order. A product of Jedi and Sith teachings, Ren embodied the conflict between the dark side and the light, making the young Force warrior dangerously unstable. Yet it was through discord that he derived power, and he learned to channel his anger into strength. Inspired by the legacy of his grandfather, the Sith Lord Darth Vader, Ren sought to destroy the last remnants of the Jedi Order and conquer the galaxy.", 35, "", "Ben Solo", 5 },
                    { new Guid("2e9f09a6-6c33-4dfc-ba56-f2c21ecceb29"), new Guid("3798efc7-4e33-40f6-b160-bcea5ff182d0"), "Jabba Desilijic Tiure, more commonly referred to as Jabba the Hutt or simply Jabba, and formally styled as His Excellency Jabba Desilijic Tiure of Nal Hutta, Eminence of Tatooine, was a Hutt gangster and crime lord, as well as a member of the Grand Hutt Council, who operated and led a criminal empire from his palace on the Outer Rim world of Tatooine. Jabba was a major figure on Tatooine, where he controlled the bulk of the trafficking in illegal goods, piracy and slavery that generated most of the planet's wealth. He was also highly influential in the entire Outer Rim as one of its most powerful crime lords.", 4, "", "Jabba the Hutt", -600 },
                    { new Guid("82ae0873-2570-4d04-adcf-72fb3d6d65cd"), new Guid("3798efc7-4e33-40f6-b160-bcea5ff182d0"), "Jango Fett was a human male Mandalorian bounty hunter and the clone template of the Grand Army of the Republic. Known as the best bounty hunter in the galaxy during the final years of the Galactic Republic, Fett was proficient in marksmanship as well as unarmed combat. The Mandalorian armor that he wore featured various weapons and gadgets, including a flamethrower, dual WESTAR-34 blaster pistols, and a jetpack. His personal starship was the Firespray-31-class patrol and attack craft Slave I.", -22, "", "Jango Fett", -66 },
                    { new Guid("2c17951d-7f63-46af-ac46-2843ded213c7"), new Guid("3798efc7-4e33-40f6-b160-bcea5ff182d0"), "Padmé Amidala, also known as Padmé Amidala Naberrie, was a human female senator who represented the people of Naboo during the final years of the Galactic Republic. Prior to her career in the Galactic Senate, Amidala was the elected ruler of the Royal House of Naboo. A political idealist, she advocated for the preservation of democracy as well as a peaceful resolution to the Clone Wars. However, her secret marriage to the Jedi Knight Anakin Skywalker would have a lasting effect on the future of the galaxy for decades to come.", 19, "", "Padme Amidala", -46 },
                    { new Guid("1d2601df-81fd-48b2-a01c-4423bc7bc0ca"), new Guid("3798efc7-4e33-40f6-b160-bcea5ff182d0"), "Han Solo rose from an impoverished childhood on the mean streets of Corellia to become one of the heroes of the Rebel Alliance. As captain of the Millennium Falcon, Han and his co-pilot Chewbacca came to believe in the cause of galactic freedom, joining Luke Skywalker and Princess Leia Organa in the fight against the Empire. After the Battle of Endor, Han faced difficult times in a chaotic galaxy, leading to a shattering confrontation with his estranged son Ben.", 34, "", "Han Solo", -32 },
                    { new Guid("51a5b333-0a25-409f-ae62-3cbc65586943"), new Guid("3798efc7-4e33-40f6-b160-bcea5ff182d0"), "Darth Sidious, born Sheev Palpatine and known simply as the Emperor, was a human male Dark Lord of the Sith who reigned over the galaxy as Galactic Emperor of the First Galactic Empire. Rising to power in the Galactic Senate as the senator of Naboo, the manipulative Sith Lord cultivated two identities, Sidious and Palpatine, using both to further his political career. He masterminded the Clone Wars in order to gain dictatorial powers during the final years of the Galactic Republic. After the fall of the Jedi Order, Sidious established a reign lasting from 19 BBY until his demise in 4 ABY. Death was not the end for Sidious, however, as the dark side of the Force was a pathway to abilities that made his return possible.", 35, "", "Luke Skylwalker", -19 },
                    { new Guid("5cd1083b-58d7-44e5-8601-03d01ff0e526"), new Guid("3798efc7-4e33-40f6-b160-bcea5ff182d0"), "Darth Sidious, born Sheev Palpatine and known simply as the Emperor, was a human male Dark Lord of the Sith who reigned over the galaxy as Galactic Emperor of the First Galactic Empire. Rising to power in the Galactic Senate as the senator of Naboo, the manipulative Sith Lord cultivated two identities, Sidious and Palpatine, using both to further his political career. He masterminded the Clone Wars in order to gain dictatorial powers during the final years of the Galactic Republic. After the fall of the Jedi Order, Sidious established a reign lasting from 19 BBY until his demise in 4 ABY. Death was not the end for Sidious, however, as the dark side of the Force was a pathway to abilities that made his return possible.", 4, "", "Sheev Palpatine", -84 },
                    { new Guid("270e16b5-bd35-4fac-b606-6d5df2d447a9"), new Guid("3798efc7-4e33-40f6-b160-bcea5ff182d0"), "Dooku, a Force-sensitive human male, was a Jedi Master that fell to the dark side of the Force and became the Dark Lord of the Sith Darth Tyranus during the final years of the Galactic Republic. After leaving the Jedi Order, he claimed the title Count of Serenno and, during the Clone Wars, served as Head of State of the Confederacy of Independent Systems. He was the second apprentice of Darth Sidious, the Dark Lord of the Sith whose plan to conquer the galaxy relied on Dooku leading a pan-galactic secessionist movement against the Republic. As such, Dooku immersed himself in the dark side and worked tirelessly to advance his and his master's plans, but ultimately forgot that treachery was the way of the Sith.", -19, "", "Count Dooku", -102 },
                    { new Guid("7a753ae0-6688-4d9b-af00-f79166d7cd17"), new Guid("3798efc7-4e33-40f6-b160-bcea5ff182d0"), "Obi-Wan Kenobi was a legendary human male Jedi Master who served on the Jedi High Council during the last years of the Republic Era. During the Imperial Era, he adopted the alias Ben Kenobi in order to hide from the regime that drove the Jedi to near extinction. A noble man known for his skills with the Force, Kenobi trained Anakin Skywalker, served as a Jedi General in the Grand Army of the Republic, and became a mentor to Luke Skywalker prior to his death in 0 BBY.", 0, "https://www.giantfreakinrobot.com/wp-content/uploads/2022/05/obi-wan-kenobi-900x506.jpg", "Obi-Wan Kenobi", -57 },
                    { new Guid("1b8ea37f-ba00-489c-8cb4-524a7d469f62"), new Guid("3798efc7-4e33-40f6-b160-bcea5ff182d0"), "Anakin was a human male Jedi Knight of the Galactic Republic and the prophesied Chosen One of the Jedi Order, destined to bring balance to the Force. Also known as Ani during his childhood, Skywalker earned the moniker Hero With No Fear from his accomplishments in the Clone Wars. His alter ego, Darth Vader, the Dark Lord of the Sith, was created when Skywalker turned to the dark side of the Force, pledging his allegiance to the Sith Lord Darth Sidious at the end of the Republic Era.", 4, "https://static.wikia.nocookie.net/starwars/images/6/6f/Anakin_Skywalker_RotS.png/revision/latest?cb=20160427145922&path-prefix=hu", "Anakin Skywalker", -41 },
                    { new Guid("66c7ff86-acf9-4dbd-a44a-053ffafd50d0"), new Guid("3798efc7-4e33-40f6-b160-bcea5ff182d0"), "Qui-Gon Jinn, a Force-sensitive human male, was a venerable if maverick Jedi Master who lived during the last years of the Republic Era. He was a wise and well-respected member of the Jedi Order, and was offered a seat on the Jedi Council, but chose to reject and follow his own path. Adhering to a philosophy centered around the Living Force, Jinn strove to follow the will of the Force even when his actions conflicted with the wishes of the High Council. After encountering Anakin Skywalker, Jinn brought him to the Jedi Temple on Coruscant, convinced he had found the Chosen One. His dying wish was for Skywalker to become a Jedi and ultimately restore balance to the Force.", -32, "https://upload.wikimedia.org/wikipedia/en/a/ad/Qui-Gon_Jinn.png", "Qui-Gon Jinn", -80 },
                    { new Guid("21a5377d-634d-43ec-8160-fc16f85c0b29"), new Guid("3798efc7-4e33-40f6-b160-bcea5ff182d0"), "Mace Windu was a revered human male Jedi Master and member of the Jedi High Council during the last years of the Galactic Republic. During his time in the Jedi Order, he once served as elected leader of the Jedi and, during the Clone Wars, as a Jedi General in the Grand Army of the Republic. He was the greatest champion of the Jedi Order and promoted its ancient traditions amidst the growing influence of the dark side of the Force in the corrupt, declining days of the Republic.", -19, "https://static.wikia.nocookie.net/starwars/images/7/7e/MaceWindu.jpg/revision/latest?cb=20180813112704&path-prefix=hu", "Mace Windu", -72 },
                    { new Guid("745279e3-f390-405c-85c3-f92b41c1221b"), new Guid("9a12c346-cf77-466d-88ce-73ea07e83f54"), "", -30000, "", "First Hyperdrive by Devaronians", -30000 },
                    { new Guid("6d26ece4-b0e2-4d27-bed7-217572ecda34"), new Guid("ceda4792-392a-4920-a5b5-9b6abe04fca5"), "", -14, "", "Jedi Fallen Order", -19 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_TimeLines_CategoryId",
                table: "TimeLines",
                column: "CategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropTable(
                name: "TimeLines");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
