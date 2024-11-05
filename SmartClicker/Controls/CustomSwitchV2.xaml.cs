using System.Windows.Input;

namespace SmartClicker.Controls
{
    [XamlCompilation(XamlCompilationOptions.Skip)]
    public partial class CustomSwitchV2 : ContentView
    {
        public CustomSwitchV2()
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
                typeof(CustomSwitchV2),
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
                typeof(CustomSwitchV2),
                null);

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        // Обработчик изменения свойства IsToggled
        private static void OnIsToggledChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is CustomSwitchV2 customSwitchV2)
            {
                customSwitchV2.UpdateAppearance();
                customSwitchV2.Command?.Execute(customSwitchV2.IsToggled);
                customSwitchV2.InvalidateMeasure(); // Принудительное обновление UI
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

            // прозрачность
            await RootView.FadeTo(0.1, 210);

            // вжим
            await RootView.ScaleTo(0.9, 100);
            await RootView.ScaleTo(1.05, 145);

            // возвращение
            await RootView.FadeTo(1, 165);
            await RootView.ScaleTo(1, 145);
        }
    }
}