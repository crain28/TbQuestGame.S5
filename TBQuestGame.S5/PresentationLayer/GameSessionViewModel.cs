using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBQuestGame.Models;
using System.Windows.Threading;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Windows;
using TBQuestGame.PresentationLayer;

namespace TBQuestGame
{
    /// <summary>
    /// view model for the game session view
    /// </summary>
    public class GameSessionViewModel : ObservableObject
    {
        #region ENUMS

        #endregion

        #region FIELDS

        private DateTime _gameStartTime;
        private string _gameTimeDisplay;
        private TimeSpan _gameTime;

        private Player _player;

        private Map _gameMap;
        private Location _currentLocation;
        private Location _northLocation, _eastLocation, _southLocation, _westLocation;
        private string _currentLocationInformation;

        private GameItemQuantity _currentGameItem;
        private Npc _currentNpc;

        private Random random = new Random();

        #endregion

        #region PROPERTIES

        public Npc CurrentNpc
        {
            get { return _currentNpc; }
            set
            {
                _currentNpc = value;
                OnPropertyChanged(nameof(CurrentNpc));
            }
        }

        public GameItemQuantity CurrentGameItem
        {
            get { return _currentGameItem; }
            set
            {
                _currentGameItem = value;
                OnPropertyChanged(nameof(CurrentGameItem));
                if (_currentGameItem != null && _currentGameItem.GameItem is Weapon)
                {
                    _player.CurrentWeapon = _currentGameItem.GameItem as Weapon;
                }
            }
        }

        public Player Player
        {
            get { return _player; }
            set { _player = value; }
        }

        public string MessageDisplay
        {
            get { return _currentLocation.Message; }
        }
        public Map GameMap
        {
            get { return _gameMap; }
            set { _gameMap = value; }
        }
        public Location CurrentLocation
        {
            get { return _currentLocation; }
            set
            {
                _currentLocation = value;
                _currentLocationInformation = _currentLocation.Description;
                OnPropertyChanged(nameof(CurrentLocation));
                OnPropertyChanged(nameof(CurrentLocationInformation));
            }
        }
        
        // expose information about travel points from current location
        public Location NorthLocation
        {
            get { return _northLocation; }
            set
            {
                _northLocation = value;
                OnPropertyChanged(nameof(NorthLocation));
                OnPropertyChanged(nameof(HasNorthLocation));
            }
        }

        public void OpenMissionStatusView()
        {
            MissionStatusView missionStatusView = new MissionStatusView(InitializeMissionStatusViewModel());
            missionStatusView.Show();
        }

        public Location EastLocation
        {
            get { return _eastLocation; }
            set
            {
                _eastLocation = value;
                OnPropertyChanged(nameof(EastLocation));
                OnPropertyChanged(nameof(HasEastLocation));
            }
        }

        public Location SouthLocation
        {
            get { return _southLocation; }
            set
            {
                _southLocation = value;
                OnPropertyChanged(nameof(SouthLocation));
                OnPropertyChanged(nameof(HasSouthLocation));
            }
        }

        public Location WestLocation
        {
            get { return _westLocation; }
            set
            {
                _westLocation = value;
                OnPropertyChanged(nameof(WestLocation));
                OnPropertyChanged(nameof(HasWestLocation));
            }
        }

        public string CurrentLocationInformation
        {
            get { return _currentLocationInformation; }
            set
            {
                _currentLocationInformation = value;
                OnPropertyChanged(nameof(CurrentLocationInformation));
            }
        }

        public bool HasNorthLocation
        {
            get
            {
                if (NorthLocation != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        
        // shortened code with same functionality as above
        public bool HasEastLocation { get { return EastLocation != null; } }
        public bool HasSouthLocation { get { return SouthLocation != null; } }
        public bool HasWestLocation { get { return WestLocation != null; } }

        public string MissionTimeDisplay
        {
            get { return _gameTimeDisplay; }
            set
            {
                _gameTimeDisplay = value;
                OnPropertyChanged(nameof(MissionTimeDisplay));
            }
        }

        #endregion

        #region CONSTRUCTORS

        const string TAB = "\t";
        const string NEW_LINE = "\t";


        public GameSessionViewModel()
        {

        }

        public GameSessionViewModel(
            Player player,
            Map gameMap,
            GameMapCoordinates currentLocationCoordinates)
        {
            _player = player;

            _gameMap = gameMap;
            _gameMap.CurrentLocationCoordinates = currentLocationCoordinates;
            _currentLocation = _gameMap.CurrentLocation;
            InitializeView();

            GameTimer();
        }

        #endregion

        #region METHODS

        private string GenerateMissionTravelDetail(MissionTravel mission)
        {
            StringBuilder sb = new StringBuilder();
            sb.Clear();

            sb.AppendLine("All Required Locations");
            foreach (var location in mission.RequiredNpcs)
            {
                sb.AppendLine(TAB + location.Name);
            }

            if (mission.Status == Mission.MissionStatus.Incomplete)
            {
                sb.AppendLine("Locations Yet to Visit");
                foreach (var location in mission.LocationsNotCompleted(_player.LocationsVisited))
                {
                    sb.AppendLine(TAB + location.Name);
                }
            }
            // remove the last two characters that generate a blank line
            sb.Remove(sb.Length - 2, 2);
            return sb.ToString(); 
        }

        private string GenerateMissionEngageDetail(MissionEngage mission)
        {
            StringBuilder sb = new StringBuilder();
            sb.Clear();

            sb.AppendLine("All Required NPCs");
            foreach (var location in mission.RequiredNpcs)
            {
                sb.AppendLine(TAB + location.Name);
            }

            if (mission.Status == Mission.MissionStatus.Incomplete)
            {
                sb.AppendLine("NPCs Yet to Engage");
                foreach (var location in mission.NpcsNotCompleted(_player.NpcsEngaged))
                {
                    sb.AppendLine(TAB + location.Name);
                }
            }
            // remove the last two characters that generate a blank line
            sb.Remove(sb.Length - 2, 2);
            return sb.ToString();
        }
        private string GenerateMissionGatherDetail(MissionGather mission)
        {
            StringBuilder sb = new StringBuilder();
            sb.Clear();

            sb.AppendLine("All Required Game Items (Quantity)");
            foreach (var gameItemQuantity in mission.RequiredGameItemQuantites)
            {
                sb.AppendLine(TAB + gameItemQuantity.GameItem.Name);
                sb.AppendLine($" ( {gameItemQuantity.Quantity} )");
            }

            if (mission.Status == Mission.MissionStatus.Incomplete)
            {
                sb.AppendLine("Game Items Yet to Gather (Quantity) ");
                foreach (var gameItemQuantity in mission.GameItemQuantitiesNotCompleted(_player.Inventory.ToList()))
                {
                    int quantityInIventory = 0;
                    GameItemQuantity gameItemQuantityGathered = _player.Inventory.FirstOrDefault(gi => gi.GameItem.Id == gameItemQuantity.GameItem.Id);
                    if (gameItemQuantityGathered != null)
                        quantityInIventory = gameItemQuantityGathered.Quantity;
                    
                    sb.AppendLine(TAB + gameItemQuantity.GameItem.Name);
                    sb.AppendLine($" ( {gameItemQuantity.Quantity - quantityInIventory} )");
                }
            }
            // remove the last two characters that generate a blank line
            sb.Remove(sb.Length - 2, 2);
            return sb.ToString();
        }

        /// <summary>
        /// generate the mission status information text based on percentage of missions completed
        /// </summary>
        /// <returns>mission status information text</returns>
        private string GenerateMissionStatusInformation()
        {
            string missionStatusInformation;

            double totalMissions = _player.Missions.Count();
            double missionsCompleted = _player.Missions.Where(m => m.Status == Mission.MissionStatus.Complete).Count();

            int percentMissionsCompleted = (int)((missionsCompleted / totalMissions) * 100);
            missionStatusInformation = $"Missions Complete: {percentMissionsCompleted}%" + NEW_LINE;

            if (percentMissionsCompleted == 0)
            {
                missionStatusInformation += "Looks like you are just starting out. No missions complete at this point and you better get on it!";
            }
            else if (percentMissionsCompleted <= 33)
            {
                missionStatusInformation += "Well, you have some of your missions complete, but this is just a start. You have your work cut out for you for sure.";
            }
            else if (percentMissionsCompleted <= 66)
            {
                missionStatusInformation += "You are making great progress with your missions. Keep at it.";
            }
            else if (percentMissionsCompleted == 100)
            {
                missionStatusInformation += "Congratulations, you have completed all missions assigned to you.";
            }

            return missionStatusInformation;
        }

        /// <summary>
        /// initialize all property values for the mission status view model
        /// </summary>
        /// <returns>mission status view model</returns>
        private MissionStatusViewModel InitializeMissionStatusViewModel()
        {
            MissionStatusViewModel missionStatusViewModel = new MissionStatusViewModel();

            missionStatusViewModel.MissionInformation = GenerateMissionStatusInformation();

            missionStatusViewModel.Missions = new List<Mission>(_player.Missions);
            foreach (Mission mission in missionStatusViewModel.Missions)
            {
                if (mission is MissionTravel)
                    mission.StatusDetail = GenerateMissionTravelDetail((MissionTravel)mission);

                if (mission is MissionEngage)
                    mission.StatusDetail = GenerateMissionEngageDetail((MissionEngage)mission);

                if (mission is MissionGather)
                    mission.StatusDetail = GenerateMissionGatherDetail((MissionGather)mission);
            }

            return missionStatusViewModel;
        }


        /// <summary>
        /// initial setup of the game session view
        /// </summary>
        private void InitializeView()
        {
            _gameStartTime = DateTime.Now;
            UpdateAvailableTravelPoints();
            _currentLocationInformation = CurrentLocation.Description;
            _player.UpdateInventoryCategories();
            _player.CalculateWealth();
        }

        /// <summary>
        /// calculate the available travel points from current location
        /// game slipstreams are a mapping against the 2D array where 
        /// </summary>
        private void UpdateAvailableTravelPoints()
        {
            // reset travel location information
            NorthLocation = null;
            EastLocation = null;
            SouthLocation = null;
            WestLocation = null;

            // north location exists
            if (_gameMap.NorthLocation() != null)
            {
                Location nextNorthLocation = _gameMap.NorthLocation();

                // location generally accessible or player has required conditions
                if (nextNorthLocation.Accessible == true || PlayerCanAccessLocation(nextNorthLocation))
                {
                    NorthLocation = nextNorthLocation;
                }
            }
            
            // east location exists
            if (_gameMap.EastLocation() != null)
            {
                Location nextEastLocation = _gameMap.EastLocation();
                
                // location generally accessible or player has required conditions
                if (nextEastLocation.Accessible == true || PlayerCanAccessLocation(nextEastLocation))
                {
                    EastLocation = nextEastLocation;
                }
            }

            // south location exists
            if (_gameMap.SouthLocation() != null)
            {
                Location nextSouthLocation = _gameMap.SouthLocation();
                
                // location generally accessible or player has required conditions
                if (nextSouthLocation.Accessible == true || PlayerCanAccessLocation(nextSouthLocation))
                {
                    SouthLocation = nextSouthLocation;
                }
            }
            
            // west location exists
            if (_gameMap.WestLocation() != null)
            {
                Location nextWestLocation = _gameMap.WestLocation();


                // location generally accessible or player has required conditions
                if (nextWestLocation.Accessible == true || PlayerCanAccessLocation(nextWestLocation))
                {
                    WestLocation = nextWestLocation;
                }
            }
        }

        /// <summary>
        /// location to check accessibility
        /// </summary>
        private bool PlayerCanAccessLocation(Location nextLocation)
        {
            //
            // check access by experience points
            //
            if (nextLocation.IsAccessibleByExperience(_player.Experience))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// player move event handler
        /// </summary>
        private void OnPlayerMove()
        {
            // update player stats when in new location
            if (!_player.HasVisited(_currentLocation))
            {
                // add location to list of visited locations
                _player.LocationsVisited.Add(_currentLocation);

                // update experience points
                _player.Experience += _currentLocation.ModifiyExperience;

                // update health
                _player.Health += _currentLocation.ModifyHealth;

                // update lives
                if (_currentLocation.ModifyLives != 0) _player.Lives += _currentLocation.ModifyLives;

                // display a new message if available
                OnPropertyChanged(nameof(MessageDisplay));
            }
        }

        /// <summary>
        /// travel north
        /// </summary>
        public void MoveNorth()
        {
            if (HasNorthLocation)
            {
                _gameMap.MoveNorth();
                CurrentLocation = _gameMap.CurrentLocation;
                UpdateAvailableTravelPoints();
                OnPlayerMove();
                _player.UpdateMissionStatus();
            }
        }

        /// <summary>
        /// travel east
        /// </summary>
        public void MoveEast()
        {
            if (HasEastLocation)
            {
                _gameMap.MoveEast();
                CurrentLocation = _gameMap.CurrentLocation;
                UpdateAvailableTravelPoints();
                OnPlayerMove();
                _player.UpdateMissionStatus();
            }
        }

        /// <summary>
        /// travel south
        /// </summary>
        public void MoveSouth()
        {
            if (HasSouthLocation)
            {
                _gameMap.MoveSouth();
                CurrentLocation = _gameMap.CurrentLocation;
                UpdateAvailableTravelPoints();
                OnPlayerMove();
                _player.UpdateMissionStatus();
            }
        }

        /// <summary>
        /// travel west
        /// </summary>
        public void MoveWest()
        {
            if (HasWestLocation)
            {
                _gameMap.MoveWest();
                CurrentLocation = _gameMap.CurrentLocation;
                UpdateAvailableTravelPoints();
                OnPlayerMove();
                _player.UpdateMissionStatus();
            }
        }


        #endregion

        #region Time Methods

        /// <summary>
        /// running time of game
        /// </summary>
        private TimeSpan GameTime()
        {
            return DateTime.Now - _gameStartTime;
        }

        /// <summary>
        /// game time event, publishes every 1 second
        /// </summary>
        public void GameTimer()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1000);
            timer.Tick += OnGameTimerTick;
            timer.Start();
        }

        /// <summary>
        /// game timer event handler
        /// 1) update mission time on window
        /// </summary>
        void OnGameTimerTick(object sender, EventArgs e)
        {
            _gameTime = DateTime.Now - _gameStartTime;
            MissionTimeDisplay = "Mission Time " + _gameTime.ToString(@"hh\:mm\:ss");
        }

        #endregion

        #region Inventory Methods

        /// <summary>
        /// add a new item to the players inventory
        /// </summary>
        /// <param name="selectedItem"></param>
        public void AddItemToInventory()
        {
            //
            // confirm a game item selected and is in current location
            // subtract from location and add to inventory
            if (_currentGameItem != null && _currentLocation.GameItems.Contains(_currentGameItem))
            {
                // cast selected game item 
                GameItemQuantity selectedGameItemQuantity = _currentGameItem as GameItemQuantity;

                _currentLocation.RemoveGameItemQuantityFromLocation(selectedGameItemQuantity);
                _player.AddGameItemQuantityToInventory(selectedGameItemQuantity);

                OnPlayerPickUp(selectedGameItemQuantity);
            }
        }

        /// <summary>
        /// remove item from the players inventory
        /// </summary>
        /// <param name="selectedItem"></param>
        public void RemoveItemFromInventory()
        { 
            // confirm a game item selected and is in inventory
            // subtract from inventory and add to location
            if (_currentGameItem != null)
            {
                // cast selected game item 
                //
                GameItemQuantity selectedGameItemQuantity = _currentGameItem as GameItemQuantity;

                _currentLocation.AddGameItemQuantityToLocation(selectedGameItemQuantity);
                _player.RemoveGameItemQuantityFromInventory(selectedGameItemQuantity);

                OnPlayerPutDown(selectedGameItemQuantity);
            }
        }

        /// <summary>
        /// process events when a player picks up a new game item
        /// </summary>
        /// <param name="gameItemQuantity">new game item</param>
        private void OnPlayerPickUp(GameItemQuantity gameItemQuantity)
        {
            _player.Experience += gameItemQuantity.GameItem.Experience;
            _player.Wealth += gameItemQuantity.GameItem.Value;
            _player.UpdateMissionStatus();
        }

        /// <summary>
        /// process events when a player puts down a new game item
        /// </summary>
        /// <param name="gameItemQuantity">new game item</param>
        private void OnPlayerPutDown(GameItemQuantity gameItemQuantity)
        {
            _player.Wealth -= gameItemQuantity.GameItem.Value;
            _player.UpdateMissionStatus();
        }

        /// <summary>
        /// process using an item in the player's inventory
        /// </summary>
        public void OnUseGameItem()
        {
            switch (_currentGameItem.GameItem)
            {
                //todo- Not Use make it Add to inventory
                case Potion potion:
                    ProcessPotionUse(potion);
                    break;
                case Loot loot:
                    ProcessLootUse(loot);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// process the effects of using the relic
        /// </summary>
        /// <param name="potion">potion</param>
        private void ProcessLootUse(Loot loot)
        {
            string message;

            switch (loot.UseAction)
            {
                case Loot.UseActionType.OPENLOCATION:
                    message = _gameMap.OpenLocationsByLoot(loot.Id);
                    CurrentLocationInformation = loot.UseMessage;
                    break;
                case Loot.UseActionType.KILLPLAYER:
                    OnPlayerDies(loot.UseMessage);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// process the effects of using the potion
        /// </summary>
        /// <param name="potion">potion</param>
        private void ProcessPotionUse(Potion potion)
        {
            _player.Health += potion.HealthChange;
            _player.Lives += potion.LivesChange;
            _player.RemoveGameItemQuantityFromInventory(_currentGameItem);
        }

        /// <summary>
        /// process player dies with option to reset and play again
        /// </summary>
        /// <param name="message">message regarding player death</param>
        private void OnPlayerDies(string message)
        {
            string messagetext = message +
                "\n\nWould you like to play again?";

            string titleText = "Death";
            MessageBoxButton button = MessageBoxButton.YesNo;
            MessageBoxResult result = MessageBox.Show(messagetext, titleText, button);

            switch (result)
            {
                case MessageBoxResult.Yes:
                    ResetPlayer();
                    break;
                case MessageBoxResult.No:
                    QuiteApplication();
                    break;
            }
        }

        /// <summary>
        /// player chooses to exit game
        /// </summary>
        private void QuiteApplication()
        {
            Environment.Exit(0);
        }

        /// <summary>
        /// player chooses to reset game
        /// </summary>
        private void ResetPlayer()
        {
            Environment.Exit(0);
        }

        #endregion

        #region Npc Methods

        /// <summary>
        /// handle the speak to event in the view
        /// </summary>
        public void OnPlayerTalkTo()
        {
            if (CurrentNpc != null && CurrentNpc is ISpeak)
            {
                ISpeak speakingNpc = CurrentNpc as ISpeak;
                CurrentLocationInformation = speakingNpc.Speak();
                _player.NpcsEngaged.Add(_currentNpc);
                _player.UpdateMissionStatus();
            }
        }
        /// <summary>
        /// handle the attack event in the view.
        /// </summary>
        public void OnPlayerAttack()
        {
            _player.BattleMode = BattleModeName.Attack;
            Battle();
            if (_currentNpc != null)
                _player.NpcsEngaged.Add(_currentNpc);
            _player.UpdateMissionStatus();
        }

        /// <summary>
        /// handle the defend event in the view.
        /// </summary>
        public void OnPlayerDefend()
        {
            _player.BattleMode = BattleModeName.Defence;
            Battle();
            if (_currentNpc != null)
                _player.NpcsEngaged.Add(_currentNpc);
            _player.UpdateMissionStatus();
        }

        /// <summary>
        /// handle the retreat event in the view.
        /// </summary>
        public void OnPlayerRetreat()
        {
            _player.BattleMode = BattleModeName.Retreat;
            Battle();
            if (_currentNpc != null)
                _player.NpcsEngaged.Add(_currentNpc);
            _player.UpdateMissionStatus();
        }
                
        private BattleModeName NpcBattleResponse()
        {
            BattleModeName npcBattleResponse = BattleModeName.Retreat;

            switch (DieRoll(3))
            {
                case 1:
                    npcBattleResponse = BattleModeName.Attack;
                    break;
                case 2:
                    npcBattleResponse = BattleModeName.Defence;
                    break;
                case 3:
                    npcBattleResponse = BattleModeName.Retreat;
                    break;
            }
            return npcBattleResponse;
        }

        private int CalculatePlayerHitPoints()
        {
            int playerHitPoints = 0;

            switch (_player.BattleMode)
            {
                case BattleModeName.Attack:
                    playerHitPoints = _player.Attack();
                    break;
                case BattleModeName.Defence:
                    playerHitPoints = _player.Defend();
                    break;
                case BattleModeName.Retreat:
                    playerHitPoints = _player.Retreat();
                    break;
            }
            return playerHitPoints;
        }

        private int CaculateNpcHitPoints(IBattle battleNpc)
        {
            int battleNpcHitPoints = 0;

            switch (NpcBattleResponse())
            {
                case BattleModeName.Attack:
                    battleNpcHitPoints = battleNpc.Attack();
                    break;
                case BattleModeName.Defence:
                    battleNpcHitPoints = battleNpc.Defend();
                    break;
                case BattleModeName.Retreat:
                    battleNpcHitPoints = battleNpc.Retreat();
                    break;
            }
            return battleNpcHitPoints;
        }

        private void Battle()
        {
            // check to see if an NPC can battle
            if (_currentNpc is IBattle)
            {
                IBattle battleNpc = _currentNpc as IBattle;
                int playerHitPoints = 0;
                int battleNpcHitPoints = 0;
                string battleInformation = "";

                // caculate hit points if the player and NPC have weapons
                if (_player.CurrentWeapon != null)
                {
                    playerHitPoints = CalculatePlayerHitPoints();
                }
                else
                {
                    battleInformation = "It appears you are entering into a battle without a weapon.";
                }

                if (battleNpc.CurrentWeapon != null)
                {
                    battleNpcHitPoints = CaculateNpcHitPoints(battleNpc);
                }
                else
                {
                    battleInformation = $"It appears you are entering into battle with {_currentNpc.Name} who has no weapon.";
                }

                // build out the text  for the current location information
                battleInformation +=
                    $"Player: {_player.BattleMode} Hit Points: {playerHitPoints}" + Environment.NewLine +
                    $"NPC: {battleNpc.BattleMode}  Hit Points: {battleNpcHitPoints}" + Environment.NewLine;

                // deterrmine results of battle
                if (playerHitPoints >= battleNpcHitPoints)
                {
                    battleInformation += $"You have killed {_currentNpc.Name}.";
                    _currentLocation.Npcs.Remove(_currentNpc);
                }
                else
                {
                    battleInformation += $"You have killed by {_currentNpc.Name}.";
                    _player.Lives--;
                }

                CurrentLocationInformation = battleInformation;
                if (_player.Lives <= 0) OnPlayerDies("You have been slain and have no lives left");

                else
                {
                    CurrentLocationInformation = "The current NPC will is not battle ready. Seems you are a bit jumpy and your experience suffers.";
                    _player.Experience -= 10;
                }
            }
        }
        
        #endregion

        #region Helper Methods

        private int DieRoll(int sides)
        {
            return random.Next(1, sides + 1);
        }


        #endregion

        #region EVENTS



        #endregion

    }
}
