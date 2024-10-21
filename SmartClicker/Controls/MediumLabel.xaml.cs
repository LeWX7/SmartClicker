using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;

namespace SmartClicker.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MediumLabel : ContentView
    {
        // Определение BindableProperty для Text
        public static readonly BindableProperty TextProperty =
            BindableProperty.Create(nameof(Text), typeof(string), typeof(MediumLabel), string.Empty);

        // Свойство Text для доступа к BindableProperty
        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public MediumLabel()
        {
            InitializeComponent();
        }
    }
}
