using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using SmartClicker.Models;
using SmartClicker.Services;
using Microsoft.Maui.Controls;
using Windows.UI.Shell;
using SmartClicker.Controls;
using Microsoft.UI.Xaml.Documents;

namespace SmartClicker.ViewModels
{
    public class MainPageViewModel : BindableObject, IDisposable
    {
        // Инициализация сервисов
        private readonly KeyboardHookService _keyboardHookService;

        private bool _isRunning;
        private CancellationTokenSource? _cancellationTokenSource;

        // Инициализация ViewModels
        public ObservableCollection<ClickBlock> ClickBlocks { get; } = new();

        public DynamicViewModel DynamicData { get; }
        public InputViewModel Input { get; }
        public SettingsViewModel Settings { get; }

        // Commands
        public ICommand AddBlocksCommand { get; }
        public ICommand ReAddBlocksCommand { get; }
        public ICommand StartCommand { get; }
        public ICommand EndCommand { get; }
        public ICommand RecordCommand { get; }

        public MainPageViewModel()
        {
            _keyboardHookService = new KeyboardHookService();
            _keyboardHookService.KeyDown += OnGlobalKeyDown;

            ClickBlocks = new ObservableCollection<ClickBlock>();
            DynamicData = new DynamicViewModel();
            Input = new InputViewModel();
            Settings = new SettingsViewModel();

            AddBlocksCommand = new Command(async () => await AddBlocksAsync());
            ReAddBlocksCommand = new Command(async () => await ReAddBlocksAsync());
            StartCommand = new Command(async () => await StartAsync());
            EndCommand = new Command(async () => await EndAsync());
            RecordCommand = new Command(async () => await RecordAsync());

            StartUpdateCursorPosition();

            Settings.SelectedUnit = "Миллисекунды";
            
        }

        private void StartUpdateCursorPosition()
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    UpdateCursorPosition();
                    await Task.Delay(100);
                }
            });
        }

        private void UpdateCursorPosition()
        {
            if (MouseService.GetCursorPos(out MouseService.POINT point))
            {
                DynamicData.CurrentX = $"X: {point.X}";
                DynamicData.CurrentY = $"Y: {point.Y}";
            }
        }

        private void OnGlobalKeyDown(object sender, int vkCode)
        {
            const int VK_E = 0x45; // E key
            const int VK_R = 0x52; // R key
            const int VK_X = 0x58; // X key
            const int VK_CONTROL = 0x11; // Control key

            if (vkCode == VK_E && (MouseService.GetAsyncKeyState(VK_CONTROL) & 0x8000) != 0) // Ctrl + E
            {
                StartCommand.Execute(null);
            }

            if (vkCode == VK_R && (MouseService.GetAsyncKeyState(VK_CONTROL) & 0x8000) != 0) // Ctrl + R
            {
                EndCommand.Execute(null);
            }

            if (vkCode == VK_X && (MouseService.GetAsyncKeyState(VK_CONTROL) & 0x8000) != 0) // Ctrl + X
            {
                RecordCommand.Execute(null);
            }
        }

        private async Task AddBlocksAsync()
        {
            int.TryParse(Input.BlockDelayEntry, out int blockDelay); int.TryParse(Input.BlockQuantityEntry, out int blockQuantity);

            if (int.TryParse(Input.BlockCountEntry, out int blockCount))
            {
                for (int i = 0; i < blockCount; i++)
                {
                    ClickBlocks.Add(new ClickBlock
                    {
                        ClickInterval = blockDelay,
                        StepScore = blockQuantity
                    });
                }
            }
        }

        private async Task ReAddBlocksAsync()
        {
            int.TryParse(Input.BlockDelayEntry, out int blockDelay); int.TryParse(Input.BlockQuantityEntry, out int blockQuantity);

            if (int.TryParse(Input.BlockCountEntry, out int blockCount))
            {
                ClickBlocks.Clear();
                for (int i = 0; i < blockCount; i++)
                {
                    ClickBlocks.Add(new ClickBlock
                    {
                        ClickInterval = blockDelay,
                        StepScore = blockQuantity
                    });
                }
            }
        }

        private async Task StartAsync()
        {
            float offset = 0;
            if (!string.IsNullOrEmpty(Input.StartOffsetEntry) && float.TryParse(Input.StartOffsetEntry, out float parsedOffset))
            {
                switch (Settings.SelectedUnit)
                {
                    case "Секунды":
                        parsedOffset *= 1000;
                        break;
                    case "с":
                        parsedOffset *= 1000;
                        break;

                    case "Минуты":
                        parsedOffset *= 1000 * 60;
                        break;

                    case "Часы":
                        parsedOffset *= 1000 * 60 * 60;
                        break;
                }


                offset = parsedOffset;
            }

            int wholeMilliseconds = (int)offset; // Целая часть задержки
            float fractionalMilliseconds = offset - wholeMilliseconds; // Дробная часть

            await Task.Delay(wholeMilliseconds);

            if (fractionalMilliseconds > 0)
            {
                // Используем SpinWait для точной обработки оставшихся миллисекунд
                await Task.Delay((int)(fractionalMilliseconds * 1000));
            }

            if (_isRunning) return;
            _isRunning = true;

            int lapScore = 1;
            if (!string.IsNullOrEmpty(Input.BlockLapEntry) && int.TryParse(Input.BlockLapEntry, out int parsedLapScore))
            {
                lapScore = parsedLapScore;
            }

            _cancellationTokenSource = new CancellationTokenSource();
            CancellationToken token = _cancellationTokenSource.Token;

            try
            {
                await Task.Run(async () =>
                {
                    for (int u = 0; u < lapScore; u++)
                    {
                        foreach (var block in ClickBlocks)
                        {
                            if (token.IsCancellationRequested) break;

                            for (int i = 0; i < block.StepScore; i++)
                            {
                                if (token.IsCancellationRequested) break;

                                MouseService.MoveCursor(block.TargetX, block.TargetY);
                                MouseService.Click(block.IsRightClick);

                                DynamicData.StepScoreLabel = $"Кликов сделано: {i + 1} / {block.StepScore}";
                                DynamicData.LapScore = $"Круг N: {u + 1} / {lapScore}";

                                await Task.Delay(block.ClickInterval, token);
                            }
                        }
                    }

                    _isRunning = false;
                }, token);
            }
            catch (OperationCanceledException)
            {
                // Обработка отмены задачи
                _isRunning = false;
            }
            catch (Exception ex)
            {
                // Обработка других возможных исключений
                System.Diagnostics.Debug.WriteLine($"Error in StartAsync: {ex.Message}");
                _isRunning = false;
            }
        }

        private async Task EndAsync()
        {
            _cancellationTokenSource?.Cancel();
            _isRunning = false;
        }

        private async Task SwitchToggledAsync(bool isToggled)
        {
            // Логика при переключении CustomSwitch
            System.Diagnostics.Debug.WriteLine($"Switch toggled to: {isToggled}");
            await Task.CompletedTask;
        }

        private async Task RecordAsync()
        {
            int.TryParse(Input.BlockDelayEntry, out int blockDelay); int.TryParse(Input.BlockQuantityEntry, out int blockQuantity);

            var block = new ClickBlock();
            MouseService.GetCursorPos(out MouseService.POINT point);
            block.TargetY = point.Y;
            block.TargetX = point.X;
            block.ClickInterval = blockDelay;
            block.StepScore = blockQuantity;

            ClickBlocks.Add(block);
        }

        public void Dispose()
        {
            _keyboardHookService.Dispose();
            _cancellationTokenSource?.Dispose();
        }
    }
}
