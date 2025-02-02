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

        private int _dynamicX;
        public int DynamicX
        {
            get => _dynamicX;
            set { _dynamicX = value; OnPropertyChanged(); }
        }

        private int _dynamicY;
        public int DynamicY
        {
            get => _dynamicY;
            set { _dynamicY = value; OnPropertyChanged(); }
        }

        public ICommand StartUpdateCursorCommand { get; }
        public ICommand UpdateCursorCommand { get; }

        public DynamicViewModel()
        {
            StartUpdateCursorCommand = new Command(StartUpdateCursorPosition);
            UpdateCursorCommand = new Command(UpdateCursorPosition);
        }

        private void StartUpdateCursorPosition()
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    UpdateCursorPosition();
                    await Task.Delay(10);
                }
            });
        }

        private void UpdateCursorPosition()
        {
            if (MouseService.GetCursorPos(out MouseService.POINT point))
            {
                DynamicX = point.X;
                CurrentX = $"X: {point.X}";

                DynamicY = point.Y;
                CurrentY = $"Y: {point.Y}";
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
