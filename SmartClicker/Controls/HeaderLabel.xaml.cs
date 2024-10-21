using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;

namespace SmartClicker.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HeaderLabel : ContentView
    {
        // Определение BindableProperty для Text
        public static readonly BindableProperty TextProperty =
            BindableProperty.Create(nameof(Text), typeof(string), typeof(HeaderLabel), string.Empty);

        // Свойство Text для доступа к BindableProperty
        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public HeaderLabel()
        {
            InitializeComponent();
        }
    }
}
