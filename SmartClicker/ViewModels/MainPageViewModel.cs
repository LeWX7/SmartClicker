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
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Runtime.CompilerServices;

namespace SmartClicker.ViewModels
{
    public class MainPageViewModel : ObservableObject
    {
        // Инициализация сервисов
        private readonly KeyboardHookService _keyboardHookService;

        private bool _isRunning;
        private bool _isPaused;
        private CancellationTokenSource? _cancellationTokenSource;
        private TaskCompletionSource<bool>? _pauseTaskCompletionSource;

        private readonly PresetService _presetService = new();
        private ObservableCollection<string> _presetFiles;
        public ObservableCollection<string> PresetFiles
        {
            get => _presetFiles;
            set => SetProperty(ref _presetFiles, value);
        }

        private string _selectedPresetFile;
        public string SelectedPresetFile
        {
            get => _selectedPresetFile;
            set => SetProperty(ref _selectedPresetFile, value);
        }

        // Инициализация ViewModels
        public ObservableCollection<ClickBlock> ClickBlocks { get; } = new();

        public DynamicViewModel DynamicData { get; }
        public InputViewModel Input { get; }
        public SettingsViewModel Settings { get; }

        public PresetData Preset { get; }

        // Commands
        public ICommand AddBlocksCommand { get; }
        public ICommand ReAddBlocksCommand { get; }
        public ICommand StartCommand { get; }
        public ICommand EndCommand { get; }
        public ICommand PauseCommand { get; }
        public ICommand RecordCommand { get; }
        public ICommand SavePresetCommand { get; }
        public ICommand LoadPresetCommand { get; }

        public bool IsPaused
        {
            get => _isPaused;
            set
            {
                if (_isPaused != value)
                {
                    _isPaused = value;
                    OnPropertyChanged();
                    TogglePauseInternal(_isPaused); // Передаём конкретное значение
                }
            }
        }

        Random random = new Random();

        public MainPageViewModel()
        {
            _keyboardHookService = new KeyboardHookService();
            _keyboardHookService.KeyDown += OnGlobalKeyDown;

            ClickBlocks = new ObservableCollection<ClickBlock>();
            DynamicData = new DynamicViewModel();
            Input = new InputViewModel();
            Settings = new SettingsViewModel();

            Preset = new PresetData();

            AddBlocksCommand = new Command(async () => await AddBlocksAsync());
            ReAddBlocksCommand = new Command(async () => await ReAddBlocksAsync());
            StartCommand = new Command(async () => await StartAsync());
            EndCommand = new Command(async () => await EndAsync());
            PauseCommand = new Command(TogglePause);
            RecordCommand = new Command(async () => await RecordAsync());
            SavePresetCommand = new Command(async () => await OnSavePresetClicked(null, null));
            LoadPresetCommand = new Command(async () => await OnLoadPresetClicked(null, null));
            
            LoadPresetList();

            DynamicData.StartUpdateCursorCommand.Execute(null);

            Settings.SelectedUnit = "Миллисекунды";
        }

        private void OnGlobalKeyDown(object sender, int vkCode)
        {
            const int VK_E = 0x45; // E key
            const int VK_R = 0x52; // R key
            const int VK_X = 0x58; // X key
            const int VK_T = 0x54; // T key
            const int VK_CONTROL = 0x11; // Ctrl key

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

            if (vkCode == VK_T && (MouseService.GetAsyncKeyState(VK_CONTROL) & 0x8000) != 0) // Ctrl + T
            {
                PauseCommand.Execute(null);
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

            IsPaused = false;

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

                                while (_isPaused)
                                {
                                    _pauseTaskCompletionSource ??= new TaskCompletionSource<bool>();
                                    await _pauseTaskCompletionSource.Task;
                                }

                                // Сохранение координат до перемещения курсора
                                int pastCoordinateX = DynamicData.DynamicX; int pastCoordinateY = DynamicData.DynamicY;

                                if (Settings.OnCoordinates)
                                { MouseService.MoveCursor(DynamicData.DynamicX, DynamicData.DynamicY); }
                                else
                                { MouseService.MoveCursor(block.TargetX, block.TargetY); }

                                if (block.IsRightClick == true)
                                { MouseService.Click(block.IsRightClick); }
                                else
                                { MouseService.Clamp(block.IsClamping); }

                                // Возвращение курсора на прошлую позицию
                                if (Settings.BackMove)
                                { MouseService.MoveCursor(pastCoordinateX, pastCoordinateY); }

                                DynamicData.StepScoreLabel = $"Кликов сделано: {i + 1} / {block.StepScore}";
                                DynamicData.LapScore = $"Круг N: {u + 1} / {lapScore}";

                                // Добавление к интервалу случайную задержку в пользовательском диапазоне
                                int interval = block.ClickInterval;

                                if (block.RandomOfDelay == 0)
                                {
                                    interval = Math.Max(0, interval + random.Next(-Input.RandomOfDelay, Input.RandomOfDelay + 1));
                                }
                                else
                                {
                                    interval = Math.Max(0, interval + random.Next(-block.RandomOfDelay, block.RandomOfDelay + 1));
                                }

                                await Task.Delay(interval, token);
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

            IsPaused = false;
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

        private void TogglePause()
        {
            IsPaused = !IsPaused;
        }

        private void TogglePauseInternal(bool isPaused)
        {
            if (!_isRunning) return;

            if (!isPaused && _pauseTaskCompletionSource != null)
            {
                _pauseTaskCompletionSource.SetResult(true);
                _pauseTaskCompletionSource = null;
            }
        }

        public void Dispose()
        {
            _keyboardHookService.Dispose();
            _cancellationTokenSource?.Dispose();
        }

        private async Task OnSavePresetClicked(object sender, EventArgs e)
        {
            Preset.ClickBlocks = ClickBlocks.ToObservableCollection();
            Preset.BlockDelayEntry = Input.BlockDelayEntry;
            Preset.BlockQuantityEntry = Input.BlockQuantityEntry;
            Preset.BlockLapEntry = Input.BlockLapEntry;
            Preset.StartOffsetEntry = Input.StartOffsetEntry;
            Preset.SelectedUnit = Settings.SelectedUnit;

            string fileName = $"preset_{DateTime.Now:yyyyMMdd_HHmmss}.json";
            await PresetService.SavePresetAsync(Preset, fileName);

            LoadPresetList();
        }

        private async Task OnLoadPresetClicked(object sender, EventArgs e)
        {
            var loadedPreset = await PresetService.LoadPresetAsync(SelectedPresetFile);

            ClickBlocks.Clear();
            foreach (var block in loadedPreset.ClickBlocks)
                ClickBlocks.Add(block);

            Input.BlockDelayEntry = loadedPreset.BlockDelayEntry;
            Input.BlockQuantityEntry = loadedPreset.BlockQuantityEntry;
            Input.BlockLapEntry = loadedPreset.BlockLapEntry;
            Input.StartOffsetEntry = loadedPreset.StartOffsetEntry;
            Settings.SelectedUnit = loadedPreset.SelectedUnit;

            LoadPresetList();
        }

        private void LoadPresetList()
        {
            var files = _presetService.GetAllPresetNames();
            PresetFiles = new ObservableCollection<string>(files);
        }
    }
}