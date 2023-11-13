using System;
using CoreBoy.cpu;

namespace CoreBoy.timer
{
    public class Timer : IAddressSpace
    {
        private readonly SpeedMode _speedMode;
        private readonly InterruptManager _interruptManager;
        private static readonly int[] FreqToBit = { 9, 3, 5, 7 };

        private int _div;
        private int _tac;
        private int _tma;
        private int _tima;
        private bool _previousBit;
        private bool _overflow;
        private int _ticksSinceOverflow;

        public Timer(InterruptManager interruptManager, SpeedMode speedMode)
        {
            this._speedMode = speedMode;
            this._interruptManager = interruptManager;
        }

        public void Tick()
        {
            this.UpdateDiv((this._div + 1) & 0xffff);
            if (!this._overflow)
            {
                return;
            }

            this._ticksSinceOverflow++;
            if (this._ticksSinceOverflow == 4)
            {
                this._interruptManager.RequestInterrupt(InterruptManager.InterruptType.Timer);
            }

            if (this._ticksSinceOverflow == 5)
            {
                this._tima = this._tma;
            }

            if (this._ticksSinceOverflow == 6)
            {
                this._tima = this._tma;
                this._overflow = false;
                this._ticksSinceOverflow = 0;
            }
        }

        private void IncTima()
        {
            this._tima++;
            this._tima %= 0x100;
            if (this._tima == 0)
            {
                this._overflow = true;
                this._ticksSinceOverflow = 0;
            }
        }

        private void UpdateDiv(int newDiv)
        {
            this._div = newDiv;
            int bitPos = FreqToBit[this._tac & 0b11];
            bitPos <<= this._speedMode.GetSpeedMode() - 1;
            bool bit = (this._div & (1 << bitPos)) != 0;
            bit &= (this._tac & (1 << 2)) != 0;
            if (!bit && this._previousBit)
            {
                this.IncTima();
            }

            this._previousBit = bit;
        }

        public bool Accepts(int address) => address >= 0xff04 && address <= 0xff07;

        public void SetByte(int address, int value)
        {
            switch (address)
            {
                case 0xff04:
                    this.UpdateDiv(0);
                    break;

                case 0xff05:
                    if (this._ticksSinceOverflow < 5)
                    {
                        this._tima = value;
                        this._overflow = false;
                        this._ticksSinceOverflow = 0;
                    }

                    break;

                case 0xff06:
                    this._tma = value;
                    break;

                case 0xff07:
                    this._tac = value;
                    break;
            }
        }

        public int GetByte(int address)
        {
            return address switch
            {
                0xff04 => this._div >> 8,
                0xff05 => this._tima,
                0xff06 => this._tma,
                0xff07 => this._tac | 0b11111000,
                _ => throw new ArgumentException()
            };
        }
    }
}