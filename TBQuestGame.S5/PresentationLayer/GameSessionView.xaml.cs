using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TBQuestGame.PresentationLayer
{
    public partial class GameSessionView : Window
    {
        GameSessionViewModel _gameSessionViewModel;

        public GameSessionView(GameSessionViewModel gameSessionViewModel)
        {
            _gameSessionViewModel = gameSessionViewModel;
            InitializeWindowTheme();
            InitializeComponent();
        }

        private void InitializeWindowTheme()
        {
            this.Title = "Giggle Root Productions";
        }

        private void NorthTravelButton_Click(object sender, RoutedEventArgs e)
        {
            _gameSessionViewModel.MoveNorth();
        }

        private void EastTravelButton_Click(object sender, RoutedEventArgs e)
        {
            _gameSessionViewModel.MoveEast();
        }

        private void WestTravelButton_Click(object sender, RoutedEventArgs e)
        {
            _gameSessionViewModel.MoveWest();
        }

        private void SouthTravelButton_Click(object sender, RoutedEventArgs e)
        {
            _gameSessionViewModel.MoveSouth();
        }

        private void PutDownButton_Click(object sender, RoutedEventArgs e)
        {
            if (PlayerDataTabControl.SelectedItem  != null)
            {
                _gameSessionViewModel.RemoveItemFromInventory();
            }
        }
        
        private void UseButton_Click(object sender, RoutedEventArgs e)
        {
            if (PlayerDataTabControl.SelectedItem != null)
            {
                _gameSessionViewModel.OnUseGameItem();
            }
        }

        private void PickUpButton_Click(object sender, RoutedEventArgs e)
        {
            if (LocationItemsDataGrid.SelectedItem != null)
            {
                _gameSessionViewModel.AddItemToInventory();
            }
        }

        private void SpeakToButton_Click(object sender, RoutedEventArgs e)
        {
            _gameSessionViewModel.OnPlayerTalkTo();
        }

        private void AttackButton_Click(object sender, RoutedEventArgs e)
        {
            _gameSessionViewModel.OnPlayerAttack();
        }

        private void DefendButton_Click(object sender, RoutedEventArgs e)
        {
            _gameSessionViewModel.OnPlayerDefend();
        }

        private void RetreatButton_Click(object sender, RoutedEventArgs e)
        {
            _gameSessionViewModel.OnPlayerRetreat();
        }

        private void MissionStatus_Click(object sender, RoutedEventArgs e)
        {
            _gameSessionViewModel.OpenMissionStatusView();
        }
    }
}