using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;
using SmartClicker.Models;
using System;
using System.Windows.Input;

namespace SmartClicker.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomSwitch : ContentView
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
        private async void UpdateAppearance()
        {
            if (IsToggled)
            {
                await Task.Delay(250);
                InnerButton.BackgroundColor = Color.FromHex("#1B1B1B");
                InnerButton.TextColor = Color.FromHex("#FFD2B6");
                InnerButton.Text = "ПКМ";
            }
            else
            {
                await Task.Delay(250);
                InnerButton.BackgroundColor = Color.FromHex("#1B1B1B");
                InnerButton.TextColor = Color.FromHex("#D2FFDE");
                InnerButton.Text = "ЛКМ";
            }
        }

        // Обработчик нажатия кнопки
        private async void OnButtonClicked(object sender, EventArgs e)
        {
            IsToggled = !IsToggled;

            // Плавное исчезновение
            await Parent.FadeTo(0.1, 210);

            await Parent.ScaleTo(0.9, 100);
            await Parent.ScaleTo(1.05, 145);

            // Возвращение к исходному состоянию
            await Parent.FadeTo(1, 165);
            await Parent.ScaleTo(1, 145);
        }
    }
}
