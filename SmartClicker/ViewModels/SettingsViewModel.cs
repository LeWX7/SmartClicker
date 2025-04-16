using SmartClicker.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SmartClicker.ViewModels
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        private string _selectedUnit;
        public string SelectedUnit
        {
            get => _selectedUnit;
            set { _selectedUnit = value; OnPropertyChanged(); }
        }

        private bool _onCoordinates;
        public bool OnCoordinates
        {
            get => _onCoordinates;
            set { _onCoordinates = value; OnPropertyChanged(); }
        }

        private bool _backMove;
        public bool BackMove
        {
            get => _backMove;
            set { _backMove = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}