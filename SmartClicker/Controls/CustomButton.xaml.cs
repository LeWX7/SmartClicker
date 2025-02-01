using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.PlatformConfiguration.GTKSpecific;
using Microsoft.Maui.Controls.Xaml;
using Microsoft.Maui.Graphics.Text;
using System.Windows.Input;

namespace SmartClicker.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomButton : ContentView
    {
        public CustomButton()
        {
            InitializeComponent();
        }

        // Определение BindableProperty для Text
        public static readonly BindableProperty TextProperty =
            BindableProperty.Create(nameof(Text), typeof(string), typeof(CustomButton), string.Empty);

        // Определение BindableProperty для Command
        public static readonly BindableProperty CommandProperty =
            BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(CustomButton), null);

        // Свойство Text для доступа к BindableProperty
        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        // Свойство Command для доступа к BindableProperty
        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        private async void OnAnimateClicked(object sender, EventArgs e)
        {
            // Анимация: сжатие
            await Parent.ScaleTo(0.97, 100);

            // Анимация: прозрачность
            await Parent.FadeTo(0.75, 200);

            // Анимация: возвращение к исходному размеру
            await Parent.FadeTo(1, 65);
            await Parent.ScaleTo(1, 145);
        }
    }
}
