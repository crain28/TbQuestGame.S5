using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBQuestGame.Models
{
    public class MissionGather : Mission
    {

        #region Fields
        
        private int _id;
        private string _name;
        private string _description;
        private MissionStatus _status;
        private string _statusDetail;
        private int _experience;
        private List<GameItemQuantity> _requiredGameItemQuantites;

        #endregion

        #region Properties

        public List<GameItemQuantity> RequiredGameItemQuantites
        {
            get { return _requiredGameItemQuantites; }
            set { _requiredGameItemQuantites = value; }
        }

        public string Name { get; set; }
        public string Description { get; set; }

        public MissionGather()
        {

        }

        public MissionGather(int id, string name, MissionStatus status, List<GameItemQuantity> requiredGameItemQuantities)
            : base(id, name, status)
        {
            _id = id;
            _name = name;
            _status = status;
            _requiredGameItemQuantites = requiredGameItemQuantities;
        }



        #endregion

        #region Methods

        public List<GameItemQuantity> GameItemQuantitiesNotCompleted(List<GameItemQuantity> inventory)
        {
            List<GameItemQuantity> gameItemQuantitiesToComplete = new List<GameItemQuantity>();

            foreach (var missionGameItem in _requiredGameItemQuantites)
            {
                GameItemQuantity inventoryItemMatch = inventory.FirstOrDefault(gi => gi.GameItem.Id == missionGameItem.GameItem.Id);

                if (inventoryItemMatch == null)
                {
                    gameItemQuantitiesToComplete.Add(missionGameItem);
                }
                else
                {
                    if (inventoryItemMatch.Quantity < missionGameItem.Quantity)
                    {
                        gameItemQuantitiesToComplete.Add(missionGameItem);
                    }
                }
            }
            return gameItemQuantitiesToComplete;
        }


        #endregion

    }
}
