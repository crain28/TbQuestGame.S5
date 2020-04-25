using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBQuestGame.Models 
{
    public class Loot : GameItem
    {
        public enum UseActionType
        {
            OPENLOCATION,
            KILLPLAYER
        }

        public UseActionType UseAction { get; set; }

        public Loot(int id, string name, int value, string description, int experiencePoints, string useMessage, UseActionType useAction)
            : base(id, name, value, description, experiencePoints, useMessage)
        {
            UseAction = useAction;
        }

        public override string InformationString()
        {
            return $"{Name}: {Description}\nValue: {Value}";
        }
    }
}