using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SmartClicker.Models
{
    public class ClickBlock : INotifyPropertyChanged
    {
        private bool _isRightClick;
        private bool _isClamping;
        private int _targetX;
        private int _targetY;
        private int _clickInterval;
        private int _stepScore;

        public bool IsRightClick
        {
            get => _isRightClick;
            set { _isRightClick = value; OnPropertyChanged(); }
        }
        
        public bool IsClamping
        {
            get => _isClamping;
            set { _isClamping = value; OnPropertyChanged(); }
        }

        public int TargetX
        {
            get => _targetX;
            set { _targetX = value; OnPropertyChanged(); }
        }

        public int TargetY
        {
            get => _targetY;
            set { _targetY = value; OnPropertyChanged(); }
        }

        public int ClickInterval
        {
            get => _clickInterval;
            set { _clickInterval = value; OnPropertyChanged(); }
        }

        public int StepScore
        {
            get => _stepScore;
            set { _stepScore = value; OnPropertyChanged(); }
        }

        private int _randomOfDelay;
        public int RandomOfDelay
        {
            get => _randomOfDelay;
            set { _randomOfDelay = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
