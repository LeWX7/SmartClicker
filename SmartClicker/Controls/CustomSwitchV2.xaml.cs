using System;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace SmartClicker.Controls
{
    public partial class CustomSwitchV2 : ContentView
    {
        public CustomSwitchV2()
        {
            InitializeComponent();
        }

        // Свойство IsToggled
        public static readonly BindableProperty IsToggledProperty =
            BindableProperty.Create(nameof(IsToggled), typeof(bool), typeof(CustomSwitchV2), false, BindingMode.TwoWay);

        public bool IsToggled
        {
            get => (bool)GetValue(IsToggledProperty);
            set => SetValue(IsToggledProperty, value);
        }

        // Свойство для команды
        public static readonly BindableProperty CommandProperty =
            BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(CustomSwitchV2), null);

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        // Настраиваемые свойства
        public static readonly BindableProperty OnTextProperty =
            BindableProperty.Create(nameof(OnText), typeof(string), typeof(CustomSwitchV2), "ВКЛ");

        public string OnText
        {
            get => (string)GetValue(OnTextProperty);
            set => SetValue(OnTextProperty, value);
        }

        public static readonly BindableProperty OffTextProperty =
            BindableProperty.Create(nameof(OffText), typeof(string), typeof(CustomSwitchV2), "ВЫКЛ");

        public string OffText
        {
            get => (string)GetValue(OffTextProperty);
            set => SetValue(OffTextProperty, value);
        }

        public static readonly BindableProperty OnBackgroundColorProperty =
            BindableProperty.Create(nameof(OnBackgroundColor), typeof(Color), typeof(CustomSwitchV2), Colors.Green);

        public Color OnBackgroundColor
        {
            get => (Color)GetValue(OnBackgroundColorProperty);
            set => SetValue(OnBackgroundColorProperty, value);
        }

        public static readonly BindableProperty OffBackgroundColorProperty =
            BindableProperty.Create(nameof(OffBackgroundColor), typeof(Color), typeof(CustomSwitchV2), Colors.Red);

        public Color OffBackgroundColor
        {
            get => (Color)GetValue(OffBackgroundColorProperty);
            set => SetValue(OffBackgroundColorProperty, value);
        }

        public static readonly BindableProperty OnTextColorProperty =
            BindableProperty.Create(nameof(OnTextColor), typeof(Color), typeof(CustomSwitchV2), Colors.White);

        public Color OnTextColor
        {
            get => (Color)GetValue(OnTextColorProperty);
            set => SetValue(OnTextColorProperty, value);
        }

        public static readonly BindableProperty OffTextColorProperty =
            BindableProperty.Create(nameof(OffTextColor), typeof(Color), typeof(CustomSwitchV2), Colors.White);

        public Color OffTextColor
        {
            get => (Color)GetValue(OffTextColorProperty);
            set => SetValue(OffTextColorProperty, value);
        }

        // Обработчик нажатия
        private async void OnButtonClicked(object sender, EventArgs e)
        {
            IsToggled = !IsToggled;
            Command?.Execute(IsToggled);

            // Анимации
            await RootView.ScaleTo(0.9, 100);
            await RootView.FadeTo(0.1, 210);
            await RootView.ScaleTo(1.05, 145);
            await RootView.FadeTo(1, 165);
            await RootView.ScaleTo(1, 145);
        }
    }
}