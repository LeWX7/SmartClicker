using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SmartClicker.Models
{
    internal class Pickers : INotifyPropertyChanged
    {


        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
