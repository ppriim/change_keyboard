using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace KeyboardLayoutSwitcher
{
    public class GlobalHotkey : IDisposable
    {
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        public const uint MOD_ALT = 0x0001;
        public const uint MOD_CONTROL = 0x0002;
        public const uint MOD_SHIFT = 0x0004;
        public const uint MOD_WIN = 0x0008;

        public event EventHandler HotKeyPressed;

        private readonly int _id;
        private readonly IntPtr _hWnd;

        public GlobalHotkey(int id, Keys key, uint modifiers, Control control)
        {
            _id = id;
            _hWnd = control.Handle;

            if (!RegisterHotKey(_hWnd, _id, modifiers, (uint)key))
            {
                throw new InvalidOperationException("Could not register hotkey.");
            }
        }

        public void OnHotKeyPressed()
        {
            HotKeyPressed?.Invoke(this, EventArgs.Empty);
        }

        public void Dispose()
        {
            UnregisterHotKey(_hWnd, _id);
            GC.SuppressFinalize(this);
        }
    }
}
