using System;
using System.Collections.Generic;
using System.Linq;

namespace TBQuestGame.Models
{
    public class MissionTravel : Mission
    {

        #region Fields

        private int _id;
        private string _name;
        private string _description;
        private MissionStatus _status;
        private string _statusDetail;
        private int _experience;
        private List<Location> _requiredLocations;


        #endregion

        #region Properties

        public List<Location> RequiredNpcs
        {
            get { return _requiredLocations; }
            set { _requiredLocations = value; }
        }

        public string Description { get; set; }
        public string Name { get; set; }

        public MissionTravel()
        {

        }

        public MissionTravel(int id, string name, MissionStatus status, List<Location> requiredlocations)
            : base(id, name, status)
        {
            _id = id;
            _name = name;
            _status = status;
            _requiredLocations = requiredlocations;
        }


        #endregion

        #region Methods

        public List<Location> LocationsNotCompleted(List<Location> locationsTraveled)
        {
            List<Location> locationsToComplete = new List<Location>();

            foreach (var requiredLocation in _requiredLocations)
            {
                if (!locationsTraveled.Any(l => l.Id == requiredLocation.Id))
                {
                    locationsToComplete.Add(requiredLocation);
                }
            }
            return locationsToComplete;
        }

       

        #endregion

    }
}
