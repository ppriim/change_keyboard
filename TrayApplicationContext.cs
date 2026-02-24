using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace KeyboardLayoutSwitcher
{
    public class TrayApplicationContext : ApplicationContext
    {
        private NotifyIcon _notifyIcon;
        private GlobalHotkey _hotKey;
        private Form _hiddenForm;

        // Hidden form to handle Windows Messages for the hotkey
        private class HiddenForm : Form
        {
            private TrayApplicationContext _context;

            public HiddenForm(TrayApplicationContext context)
            {
                _context = context;
                this.WindowState = FormWindowState.Minimized;
                this.ShowInTaskbar = false;
                this.Visible = false;
            }

            protected override void WndProc(ref Message m)
            {
                if (m.Msg == 0x0312) // WM_HOTKEY
                {
                    _context._hotKey.OnHotKeyPressed();
                }
                base.WndProc(ref m);
            }
        }

        public TrayApplicationContext()
        {
            _hiddenForm = new HiddenForm(this);
            _hiddenForm.Show();
            _hiddenForm.Hide();

            // Setup Tray Icon
            _notifyIcon = new NotifyIcon()
            {
                Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath),
                ContextMenuStrip = new ContextMenuStrip(),
                Text = "Keyboard Layout Switcher (Ctrl+Shift+X)",
                Visible = true
            };

            _notifyIcon.ContextMenuStrip.Items.Add("Settings...", null, (s, e) => new Form1().ShowDialog());
            _notifyIcon.ContextMenuStrip.Items.Add("About", null, (s, e) => MessageBox.Show("Keyboard Layout Switcher\nHotkey: Ctrl+Shift+X", "About"));
            _notifyIcon.ContextMenuStrip.Items.Add(new ToolStripSeparator());
            _notifyIcon.ContextMenuStrip.Items.Add("Exit", null, (s, e) => ExitThread());

            // Register Hotkey: Ctrl + Shift + X
            try
            {
                _hotKey = new GlobalHotkey(1, Keys.X, GlobalHotkey.MOD_CONTROL | GlobalHotkey.MOD_SHIFT, _hiddenForm);
                _hotKey.HotKeyPressed += OnHotKeyPressed;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to register hotkey: " + ex.Message);
            }
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(int vKey);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);

        private const byte VK_SHIFT = 0x10;
        private const byte VK_CONTROL = 0x11;
        private const byte VK_C = 0x43;
        private const byte VK_V = 0x56;
        private const uint KEYEVENTF_KEYUP = 0x0002;

        private void SimulateCopy()
        {
            // Ensure modifiers are "down"
            keybd_event(VK_CONTROL, 0, 0, 0);
            Thread.Sleep(50);
            keybd_event(VK_C, 0, 0, 0);
            Thread.Sleep(50);
            keybd_event(VK_C, 0, KEYEVENTF_KEYUP, 0);
            Thread.Sleep(50);
            keybd_event(VK_CONTROL, 0, KEYEVENTF_KEYUP, 0);
        }

        private void SimulatePaste()
        {
            // Ensure modifiers are "down"
            keybd_event(VK_CONTROL, 0, 0, 0);
            Thread.Sleep(50);
            keybd_event(VK_V, 0, 0, 0);
            Thread.Sleep(50);
            keybd_event(VK_V, 0, KEYEVENTF_KEYUP, 0);
            Thread.Sleep(50);
            keybd_event(VK_CONTROL, 0, KEYEVENTF_KEYUP, 0);
        }

        private async void OnHotKeyPressed(object sender, EventArgs e)
        {
            // IMPORTANT: Wait for the user to release ALL keys of the hotkey (Ctrl, Shift, X)
            // This replicates the "delay" caused by the MessageBox in Debug mode
            while ((GetAsyncKeyState((int)Keys.ControlKey) & 0x8000) != 0 || 
                   (GetAsyncKeyState((int)Keys.ShiftKey) & 0x8000) != 0 || 
                   (GetAsyncKeyState((int)Keys.X) & 0x8000) != 0)
            {
                Thread.Sleep(10);
            }
            
            // Extra safety delay
            Thread.Sleep(100);

            // Force release modifiers just in case GetAsyncKeyState missed it or for other apps
            keybd_event(VK_CONTROL, 0, KEYEVENTF_KEYUP, 0);
            keybd_event(VK_SHIFT, 0, KEYEVENTF_KEYUP, 0);

            // Safer Clipboard operations with retries
            void SafeClipboardAction(Action action)
            {
                for (int i = 0; i < 10; i++)
                {
                    try { action(); return; }
                    catch { Thread.Sleep(50); }
                }
            }

            // 1. Clear clipboard
            SafeClipboardAction(() => Clipboard.Clear());
            
            // 2. Simulate Ctrl+C
            SimulateCopy();
            
            // 3. Get text with retry
            string originalText = null;
            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(50);
                try 
                {
                    if (Clipboard.ContainsText())
                    {
                        originalText = Clipboard.GetText();
                        if (!string.IsNullOrEmpty(originalText)) break;
                    }
                }
                catch { /* Retry */ }
            }
            
            if (string.IsNullOrEmpty(originalText)) return;

            // 4. Convert text (Using AI Agent if enabled, falls back to KeyboardMapper otherwise)
            string convertedText = await AIAgent.CorrectTextAsync(originalText);

            if (originalText != convertedText)
            {
                // 5. Set text with retry
                SafeClipboardAction(() => Clipboard.SetText(convertedText));
                
                // 6. Delay and Paste
                Thread.Sleep(150);
                SimulatePaste();
            }
        }

        protected override void ExitThreadCore()
        {
            _hotKey?.Dispose();
            _notifyIcon.Visible = false;
            _notifyIcon.Dispose();
            _hiddenForm.Dispose();
            base.ExitThreadCore();
        }
    }
}
