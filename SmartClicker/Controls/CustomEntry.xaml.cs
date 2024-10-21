using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;

namespace SmartClicker.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomEntry : ContentView
    {
        // Определение BindableProperty для Text с TwoWay привязкой
        public static readonly BindableProperty TextProperty =
            BindableProperty.Create(nameof(Text), typeof(string), typeof(CustomEntry), string.Empty, BindingMode.TwoWay);

        // Определение BindableProperty для Placeholder с OneWay привязкой
        public static readonly BindableProperty PlaceholderProperty =
            BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(CustomEntry), string.Empty, BindingMode.OneWay);

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public string Placeholder
        {
            get => (string)GetValue(PlaceholderProperty);
            set => SetValue(PlaceholderProperty, value);
        }

        public CustomEntry()
        {
            InitializeComponent();
        }
    }
}
