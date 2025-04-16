using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SmartClicker.Models
{
    public class PresetData : INotifyPropertyChanged
    {
        private ObservableCollection<ClickBlock> _clickBlocks;
        private string _startOffsetEntry;
        private string _blockDelayEntry; //стандартная задержка
        private string _blockQuantityEntry; //стандартное кол-во нажатий
        private string _selectedUnit;
        private bool _onCoordinates;
        private bool _backMove;
        private string _blockLapEntry; //кол-во кругов

        public PresetData()
        {
            _clickBlocks = new ObservableCollection<ClickBlock>();
        }

        public ObservableCollection<ClickBlock> ClickBlocks
        {
            get => _clickBlocks;
            set { _clickBlocks = value; OnPropertyChanged(); }
        }

        public string StartOffsetEntry
        {
            get => _startOffsetEntry;
            set { _startOffsetEntry = value; OnPropertyChanged(); }
        }

        public string BlockDelayEntry
        {
            get => _blockDelayEntry;
            set { _blockDelayEntry = value; OnPropertyChanged(); }
        }

        public string BlockQuantityEntry
        {
            get => _blockQuantityEntry;
            set { _blockQuantityEntry = value; OnPropertyChanged(); }
        }

        public string SelectedUnit
        {
            get => _selectedUnit;
            set { _selectedUnit = value; OnPropertyChanged(); }
        }

        public bool OnCoordinates
        {
            get => _onCoordinates;
            set { _onCoordinates = value; OnPropertyChanged(); }
        }

        public bool BackMove
        {
            get => _backMove;
            set { _backMove = value; OnPropertyChanged(); }
        }

        public string BlockLapEntry
        {
            get => _blockLapEntry;
            set { _blockLapEntry = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
