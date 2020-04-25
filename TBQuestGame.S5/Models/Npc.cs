using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBQuestGame.Models;

namespace TBQuestGame.Models
{
    public abstract class Npc : Character
    {
        public string Description { get; set; }
        public string Information
        {
            get
            {
                return InformationText();
            }
            set
            {

            }
        }

        public Npc()
        {

        }

        public Npc(int id, string name, ClanType clan, string description)
            :base(id, name, clan)
        {
            Id = id;
            Name = name;
            Clan = clan;
            Description = description;

        }

        protected abstract string InformationText();
    }
}
