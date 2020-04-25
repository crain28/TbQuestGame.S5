using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBQuestGame.Models
{
    public abstract class GameItem
    {
        // auto implemented properties are used
        public int Id { get; set; }
        public string Name { get; set; }
        public int Value { get; set; }
        public string Description { get; set; }
        public int Experience { get; set; }
        public string UseMessage { get; set; }

        public string Information
        {
            get
            {
                return InformationString();
            }
        }

        public GameItem(int id, string name, int value, string description, int experience, string useMessage = "")
        {
            Id = id;
            Name = name;
            Value = value;
            Description = description;
            Experience = experience;
            UseMessage = useMessage;
        }

        public abstract string InformationString();
    }
}
