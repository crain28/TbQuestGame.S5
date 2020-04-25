using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBQuestGame.Models
{
    public class Villager : Npc, ISpeak
    {
        public List<string> Messages { get; set; }

        protected override string InformationText()
        {
            return $"{Name} - {Description}";
        }

        public Villager()
        {

        }

        public Villager(int id, string name, ClanType clan, string description, List<string> messages)
            :base(id, name, clan, description)
        {
            Messages = messages;
        }

        /// <summary>
        /// generate select a message or use default
        /// </summary>
        public string Speak()
        {
            if (this.Messages != null)
            {
                return GetMessage();
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// randomly select a message from a list of messages
        /// </summary>
        private string GetMessage()
        {
            Random random = new Random();
            int messageIndex = random.Next(0, Messages.Count());
            return Messages[messageIndex];
        } 

    }
}
