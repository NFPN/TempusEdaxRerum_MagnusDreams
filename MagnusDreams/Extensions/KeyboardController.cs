using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using System.Collections.Generic;

namespace MagnusDreams.Extensions
{
    class KeyboardController
    {
        public event EventHandler KeyboardTick;
        public DispatcherTimer timer {get;set;}
        private HashSet<Key> pressedKeys;
        private readonly object pressedKeysLock = new object();

        public KeyboardController(Window c)
        {
            c.KeyDown += WinKeyDown;
            c.KeyUp += WinKeyUp;
            pressedKeys = new HashSet<Key>();

            timer = new DispatcherTimer();
            timer.Tick += Keyboard_Tick;
            timer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            timer.Start();
        }

        public bool KeyDown(Key key)
        {
            lock (pressedKeysLock)
            {
                return pressedKeys.Contains(key);
            }
        }

        private void WinKeyDown(object sender, KeyEventArgs e)
        {
            lock (pressedKeysLock)
            {
                pressedKeys.Add(e.Key);
            }
        }

        private void WinKeyUp(object sender, KeyEventArgs e)
        {
            lock (pressedKeysLock)
            {
                pressedKeys.Remove(e.Key);
            }
        }

        private void Keyboard_Tick(object sender, EventArgs e)
        {
            KeyboardTick?.Invoke(this, EventArgs.Empty);
        }
    }
}
