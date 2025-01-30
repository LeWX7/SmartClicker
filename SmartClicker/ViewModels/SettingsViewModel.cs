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

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}