using System;
using SmartClicker.Helpers;

namespace SmartClicker.Services
{
    public class KeyboardHookService : IDisposable
    {
        private readonly GlobalKeyboardHook _globalKeyboardHook;

        public event EventHandler<int> KeyDown;

        public KeyboardHookService()
        {
            _globalKeyboardHook = new GlobalKeyboardHook();
            _globalKeyboardHook.KeyDown += OnKeyDown;
        }

        private void OnKeyDown(object sender, int vkCode)
        {
            KeyDown?.Invoke(this, vkCode);
        }

        public void Dispose()
        {
            _globalKeyboardHook.Dispose();
        }
    }
}
