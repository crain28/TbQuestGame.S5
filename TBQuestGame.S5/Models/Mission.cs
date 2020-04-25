using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBQuestGame.Models
{
    public class Mission
    {

        #region Enum
                
        public enum MissionStatus
        {
            Unassigned,
            Incomplete,
            Complete
        }
        #endregion

        #region Properties

        private int _id;
        private string _name;
        private string _description;
        private MissionStatus _status;
        private string _statusDetail;
        private int _experience;
        internal MissionStatus Status;
        internal int Experience;
        internal int Id;

        #endregion

        #region Fields

        public Mission()
        {

        }
        public Mission(int id, string name, MissionStatus status)
        {
            _id = id;
            _name = name;
            _status = status;
        }

        public string StatusDetail
        {
            get { return _statusDetail; }
            set { _statusDetail = value; }
        }

        #endregion

    }
}
