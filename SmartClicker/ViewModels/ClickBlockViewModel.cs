using SmartClicker.Models;
using SmartClicker.Services;
using System.Windows.Input;

namespace SmartClicker.ViewModels
{
    public class ClickBlockViewModel : MainPageViewModel
    {
        private ClickBlock _clickBlock;

        public ClickBlockViewModel(ClickBlock block)
        {
            _clickBlock = block;
        }

        public bool IsRightClick
        {
            get => _clickBlock.IsRightClick;
            set
            {
                _clickBlock.IsRightClick = value;
                OnPropertyChanged();
            }
        }

        public int TargetX
        {
            get => _clickBlock.TargetX;
            set
            {
                _clickBlock.TargetX = value;
                OnPropertyChanged();
            }
        }

        public int TargetY
        {
            get => _clickBlock.TargetY;
            set
            {
                _clickBlock.TargetY = value;
                OnPropertyChanged();
            }
        }

        public int ClickInterval
        {
            get => _clickBlock.ClickInterval;
            set
            {
                _clickBlock.ClickInterval = value;
                OnPropertyChanged();
            }
        }

        public int StepScore
        {
            get => _clickBlock.StepScore;
            set
            {
                _clickBlock.StepScore = value;
                OnPropertyChanged();
            }
        }

        private string _currentX;
        public string CurrentX
        {
            get => _currentX;
            set
            {
                _currentX = value;
                OnPropertyChanged();
            }
        }

        private string _currentY;
        public string CurrentY
        {
            get => _currentY;
            set
            {
                _currentY = value;
                OnPropertyChanged();
            }
        }
    }
}