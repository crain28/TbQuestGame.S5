using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBQuestGame.Models;

/// <summary>
/// Game Data Class
/// </summary>
namespace TBQuestGame.DataLayer
{
    class GameData
    {
        #region Player Info

        public static Player PlayerData()
        {
            return new Player()
            {
                Id = 1,
                Name = "Zane",
                Age = 20,
                Title = Player.TitleName.Adventurer,
                Clan = Player.ClanType.HalfHuman,
                Health = 100,
                Lives = 3,
                Experience = 0,
                SkillLevel = 5,
                LocationId = 0,
                Inventory = new ObservableCollection<GameItemQuantity>()
                {
                    new GameItemQuantity(GameItemById(1002), 1), // Sword of Strength
                    new GameItemQuantity(GameItemById(2001), 5) // Gold Coin
                },
                Missions = new ObservableCollection<Mission>()
                {
                    MissionById(1),
                    MissionById(2),
                    MissionById(3)
                }

            };
        }


        #endregion
        
        #region Npcs Methods

        public static List<Npc> Npcs()
        {
            return new List<Npc>()
            {
                new Demon()
                {
                    Id = 2001,
                    Name = "Dragoon Styles",
                    Age = 67,
                    Clan = Character.ClanType.Demon,
                    Description = "A demon looking to strive for great things despite his heratage.",
                    Messages = new List<string>()
                    {
                        "To those who have dispised ",
                        "me I shall not live to prove you right. ",
                        "But i will live to prove you wrong"
                    },
                    SkillLevel = 8,
                   Weapon = new List<Weapon>()
                   {
                       GameItemById(1001) as Weapon
                   }
                },

                new Villager()
                {
                    Id = 1001,
                    Name = "Karla Vera",
                    Clan = Character.ClanType.Human,
                    Description = "A young women, smart and sweet, looking for her future. Never missing the opertunity to help someone else acheve there dreams ",
                    Messages = new List<string>()
                    {
                        "Hello, I'm Karla how can I help you."
                    }
                },

                new Villager()
                {
                    Id = 1002,
                    Name = "Preist Gabriel",
                    Clan = Character.ClanType.Human,
                    Description = "Preist of Altaya Temple, a generous man filled with faith.",
                    Messages = new List<string>()
                    {
                        "Welcome to Altaya Temple, anything I can help you with child?"
                    }
                }
            };
        }


        #endregion

        #region Mission Methods

        public static List<Mission> Missions()
        {
            return new List<Mission>()
            {
                new MissionTravel()
                {
                    Id = 1,
                    Name = "Scouting",
                    Description = "Explore all locations and gather all information possible",
                    Status = Mission.MissionStatus.Incomplete,
                    RequiredNpcs = new List<Location>()
                    {
                        LocationById(2),
                        LocationById(3)
                    },
                    Experience = 100
                },

                new MissionGather()
                {
                    Id = 2,
                    Name = "Collecting",
                    Description = "Locate and collect all required objects",
                    Status = Mission.MissionStatus.Incomplete,
                    RequiredGameItemQuantites = new List<GameItemQuantity>()
                    {
                        new GameItemQuantity(GameItemById(1002), 1)
                    }

                }
                
            };


        }

        private static Mission MissionById(int id)
        {
            return Missions().FirstOrDefault(m => m.Id == id);
        }

        private static Location LocationById(int id)
        {
            List<Location> locations = new List<Location>();

            foreach (Location location in GameMap().MapLocations)
            {
                if (location != null) locations.Add(location);
            }

            return locations.FirstOrDefault(i => i.Id == id);
        }



        #endregion

        #region Helper

        private static GameItem GameItemById(int id)
        {
            return StandardGameItems().FirstOrDefault(i => i.Id == id);
        }

        private static Npc NpcById(int id)
        {
            return Npcs().FirstOrDefault(i => i.Id == id);
        }

        public static GameMapCoordinates InitialGameMapLocation()
        {
            return new GameMapCoordinates() { Row = 0, Column = 0 };
        }

        #endregion

        #region Map Locations

        public static Map GameMap()
        {
            int rows = 2;
            int columns = 2;

            Map gameMap = new Map(rows, columns);

            // row 1
            gameMap.MapLocations[0, 0] = new Location()
            {
                Id = 1,
                Name = "Town of MillStone",
                Description = "The Town of MillStone Is souronded by fields and forest that the people can thrive on. " + " " +
                "But Beasts also lerk in the dark so beware. ",
                Accessible = true,
                ModifiyExperience = 10,
                GameItems = new ObservableCollection<GameItemQuantity>
                {
                    new GameItemQuantity(GameItemById(3001), 1) // Elixar-Potion
                },
                Npcs = new ObservableCollection<Npc>()
                {
                    NpcById(1001)
                }
            };

            gameMap.MapLocations[0, 1] = new Location()
            {
                Id = 2,
                Name = "Yelma Forest",
                Description = "The Yelma Forest holds both treasure and reward. " +
                "But also treachery for wondering Adventurers looking for said treasure and rewards.",
                Accessible = true,
                ModifyLives = -1,
                ModifiyExperience = +20,
                GameItems = new ObservableCollection<GameItemQuantity>
                {
                    new GameItemQuantity(GameItemById(4001), 1), // Astral Key
                    new GameItemQuantity(GameItemById(3001), 1) // Elixar-Potion
                },
                Npcs = new ObservableCollection<Npc>()
                {
                    NpcById(2001)
                }
            };

            // row 2
            gameMap.MapLocations[1, 0] = new Location()
            {
                Id = 3,
                Name = "Fenral field",
                Description = "The Fenral field is a common destination for Adventurers." +
                "Is have some farms and wild animals across the way. ",
                Accessible = true,
                ModifiyExperience = +20,
                GameItems = new ObservableCollection<GameItemQuantity>
                {
                   new GameItemQuantity(GameItemById(4002), 1), // Staff of Thorfin
                   new GameItemQuantity(GameItemById(2002), 1), // Diamond Coin
                   new GameItemQuantity(GameItemById(2001), 5), // Gold Coin
                }
            };

            gameMap.MapLocations[1, 1] = new Location()
            {
                Id = 4,
                Name = "Temple of Altaya",
                Description = "The Temple of Altaya is said to be built by demons and angels," +
                "but they have been at war for centurys. ",
                Accessible = false,
                RequiredLootId = 4001, // Astral Key
                ModifiyExperience = 50,
                ModifyLives = 3,
                RequiredExperience = 40,
                GameItems = new ObservableCollection<GameItemQuantity>
                   {
                    new GameItemQuantity(GameItemById(1001), 1), // Duel Daggers
                    new GameItemQuantity(GameItemById(3001), 1), // Elixar-Potion
                    new GameItemQuantity(GameItemById(2003), 1) // Scroll of Athens
                   },
                Npcs = new ObservableCollection<Npc>()
                {
                    NpcById(1002)
                }
            };
            return gameMap;
        }

        public static List<GameItem> StandardGameItems()
        {
            return new List<GameItem>()
            {
                new Weapon(1001, "Duel Daggers", 75, 1, 4, "Duel Daggers used for fast and agile attacks.", 10),
                new Weapon(1002, "Sword of Strength", 250, 1, 9, "A Sword that that amplifies the strength of the holder.", 20),


                new Treasure(2001, "Gold Coin", 10, Treasure.TreasureType.Gold, "Worth 24 karat gold coin", 1),
                new Treasure(2002, "Diamond Coin", 50, Treasure.TreasureType.Jewel, "A diamond shaped like a arrow head and quite valuable.", 1),
                new Treasure(2003, "Scroll of Athens", 10, Treasure.TreasureType.Scroll, "Said to be stolen by the Half Humans to advance there culture and Clan to the next level. ", 5),

                new Potion(3001, "Elixer of Satharia", 5, 40, 1, "A rare elixer from the Human clan extracted from the blood of Angels. Add 40 points of health.", 5),

                new Loot(4001, "Astral Key", 5, "Conjured by the Demon Clan to open Doors, hiding something you disire.", 5, "You have opened the Temple of Altaya", Loot.UseActionType.OPENLOCATION),
                new Loot(4002, "Staff of Thorfin", 5, "A metal staff enchanted by demons with a dark aura surrounding the staff", 5, "Grabbing the staff, you see shadows emerge from the staff and they pahse through you and you die.", Loot.UseActionType.KILLPLAYER)
            };
        }

        #endregion
        
    }
}
