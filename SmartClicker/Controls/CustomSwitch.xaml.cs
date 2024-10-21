using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;
using System;
using System.Windows.Input;

namespace SmartClicker.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomSwitch : Button
    {
        public CustomSwitch()
        {
            InitializeComponent();
            // Устанавливаем начальные значения
            UpdateAppearance();
        }

        // BindableProperty для IsToggled
        public static readonly BindableProperty IsToggledProperty =
            BindableProperty.Create(
                nameof(IsToggled),
                typeof(bool),
                typeof(CustomSwitch),
                false,
                BindingMode.TwoWay,
                propertyChanged: OnIsToggledChanged);

        // Свойство для привязки
        public bool IsToggled
        {
            get => (bool)GetValue(IsToggledProperty);
            set => SetValue(IsToggledProperty, value);
        }

        // BindableProperty для Command
        public static readonly BindableProperty CommandProperty =
            BindableProperty.Create(
                nameof(Command),
                typeof(ICommand),
                typeof(CustomSwitch),
                null);

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        // Обработчик изменения свойства IsToggled
        private static void OnIsToggledChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is CustomSwitch customSwitch)
            {
                customSwitch.UpdateAppearance();
                customSwitch.Command?.Execute(customSwitch.IsToggled);
            }
        }

        // Метод обновления внешнего вида кнопки
        private void UpdateAppearance()
        {
            if (IsToggled)
            {
                BackgroundColor = Color.FromHex("#1B1B1B");
                TextColor = Color.FromHex("#FFD2B6");
                Text = "ПКМ";
            }
            else
            {
                BackgroundColor = Color.FromHex("#1B1B1B");
                TextColor = Color.FromHex("#D2FFDE");
                Text = "ЛКМ";
            }
        }

        // Обработчик нажатия кнопки
        private void OnButtonClicked(object sender, EventArgs e)
        {
            IsToggled = !IsToggled;
        }
    }
}
