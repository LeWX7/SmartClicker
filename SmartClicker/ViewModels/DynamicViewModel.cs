using SmartClicker.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace SmartClicker.ViewModels
{
    public class DynamicViewModel : INotifyPropertyChanged
    {
        private string _currentX;
        public string CurrentX
        {
            get => _currentX;
            set { _currentX = value; OnPropertyChanged(); }
        }

        private string _currentY;
        public string CurrentY
        {
            get => _currentY;
            set { _currentY = value; OnPropertyChanged(); }
        }

        private string _lapScore;
        public string LapScore
        {
            get => _lapScore;
            set { _lapScore = value; OnPropertyChanged(); }
        }

        private string _stepScoreLabel;
        public string StepScoreLabel
        {
            get => _stepScoreLabel;
            set { _stepScoreLabel = value; OnPropertyChanged(); }
        }

        public ICommand UpdateCursorCommand { get; }

        public DynamicViewModel()
        {
            UpdateCursorCommand = new Command(UpdateCursorPosition);
        }

        private void UpdateCursorPosition()
        {
            if (MouseService.GetCursorPos(out MouseService.POINT point))
            {
                CurrentX = $"X: {point.X}";
                CurrentY = $"Y: {point.Y}";
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
