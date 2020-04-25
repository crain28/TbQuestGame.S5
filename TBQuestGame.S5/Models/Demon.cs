using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBQuestGame.Models
{
    public class Demon : Npc, ISpeak, IBattle
    {
        private const int Defender_Damage_Adjustment = 10;
        private const int Maximum_Retreat_Damage = 10;

        public List<string> Messages { get; set; }
        public int SkillLevel { get; set; }
        public BattleModeName BattleMode { get; set; }
        public Weapon CurrentWeapons { get; set; }
        public Weapon CurrentWeapon { get; set; }
        public List<Weapon> Weapon { get; set; }

        protected override string InformationText()
        {
            return $"{Name} - {Description}";
        }

        public Demon()
        {

        }

        public Demon(
            int id,
            string name,
            ClanType clan,
            string description,
            List<string> messages,
            int skillLevel,
            Weapon currentWeapons,
            Weapon currentWeapon)
            : base(id, name, clan, description)
        {
            Messages = messages;
            SkillLevel = skillLevel;
            CurrentWeapon = CurrentWeapons;
            CurrentWeapon = CurrentWeapon;
        }
        /// <summary>
        /// generate a message or use default
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
        /// randomly select a message from the list of messages
        /// </summary>
        private string GetMessage()
        {
            Random random = new Random();
            int messageIndex = random.Next(0, Messages.Count());
            return Messages[messageIndex];
        } 

        /// <summary>
        /// return hit points [0-100] based on the NPCs weapon and skill level
        /// </summary>
        public int Attack()
        {
            int hitPoints = random.Next(CurrentWeapon.MinimumDamage, CurrentWeapon.MaximumDamage) * SkillLevel;

            if (hitPoints <= 100)
            {
                return hitPoints;
            }
            else
            {
                return 100;
            }
        }

        /// <summary>
        /// return hit points [0-100] based on the NPCs weapon and skill level
        /// adjusted to deliver more damage when first attacked
        /// </summary>
        public int Defend()
        {
            int hitPoints = (random.Next(CurrentWeapon.MinimumDamage, CurrentWeapon.MaximumDamage) * SkillLevel) - Defender_Damage_Adjustment;

            if (hitPoints >= 0 && hitPoints <= 0)
            {
                return hitPoints;
            }
            else if(hitPoints > 100)
            {
                return 100;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// return hit points [0-100] based on the NPCs skill level
        /// </summary>
        public int Retreat()
        {
            int hitPoints = SkillLevel * Maximum_Retreat_Damage;

            if (hitPoints <= 100)
            {
                return hitPoints;
            }
            else
            {
                return 100;
            }
        }
    }
}
