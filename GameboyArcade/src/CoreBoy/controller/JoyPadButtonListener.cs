using System.Collections.Concurrent;
using CoreBoy.cpu;

namespace CoreBoy.controller
{
    public class JoyPadButtonListener : IButtonListener
    {
        private readonly InterruptManager _interruptManager;
        private readonly ConcurrentDictionary<Button, Button> _buttons;

        public JoyPadButtonListener(InterruptManager interruptManager, ConcurrentDictionary<Button, Button> buttons)
        {
            this._interruptManager = interruptManager;
            this._buttons = buttons;
        }

        public void OnButtonPress(Button button)
        {
            if (button != null)
            {
                this._interruptManager.RequestInterrupt(InterruptManager.InterruptType.P1013);
                this._buttons.TryAdd(button, button);
            }
        }

        public void OnButtonRelease(Button button)
        {
            if (button != null)
            {
                this._buttons.TryRemove(button, out _);
            }
        }
    }
}