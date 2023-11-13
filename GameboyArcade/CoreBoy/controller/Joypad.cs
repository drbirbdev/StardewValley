using System.Collections.Concurrent;
using CoreBoy.cpu;

namespace CoreBoy.controller
{
    public class Joypad : IAddressSpace
    {
        private readonly ConcurrentDictionary<Button, Button> Buttons = new ConcurrentDictionary<Button, Button>();
        private int P1;

        public Joypad(InterruptManager interruptManager, IController controller)
        {
            controller.SetButtonListener(new JoyPadButtonListener(interruptManager, this.Buttons));
        }

        public bool Accepts(int address)
        {
            return address == 0xff00;
        }


        public void SetByte(int address, int value)
        {
            this.P1 = value & 0b00110000;
        }

        public int GetByte(int address)
        {
            int result = this.P1 | 0b11001111;
            foreach (var b in this.Buttons.Keys)
            {
                if ((b.Line & this.P1) == 0)
                {
                    result &= 0xff & ~b.Mask;
                }
            }

            return result;
        }
    }
}
