using System;
using CoreBoy.memory;

namespace CoreBoy.sound
{
    public class SoundMode3 : SoundModeBase
    {
        private static readonly int[] DmgWave =
        {
            0x84, 0x40, 0x43, 0xaa, 0x2d, 0x78, 0x92, 0x3c,
            0x60, 0x59, 0x59, 0xb0, 0x34, 0xb8, 0x2e, 0xda
        };

        private static readonly int[] CgbWave =
        {
            0x00, 0xff, 0x00, 0xff, 0x00, 0xff, 0x00, 0xff,
            0x00, 0xff, 0x00, 0xff, 0x00, 0xff, 0x00, 0xff
        };

        private readonly Ram _waveRam = new Ram(0xff30, 0x10);
        private int _freqDivider;
        private int _lastOutput;
        private int _i;
        private int _ticksSinceRead = 65536;
        private int _lastReadAddress;
        private int _buffer;
        private bool _triggered;

        public SoundMode3(bool gbc) : base(0xff1a, 256, gbc)
        {
            foreach (var v in gbc ? CgbWave : DmgWave)
            {
                this._waveRam.SetByte(0xff30, v);
            }
        }

        public override bool Accepts(int address) => this._waveRam.Accepts(address) || base.Accepts(address);

        public override int GetByte(int address)
        {
            if (!this._waveRam.Accepts(address))
            {
                return base.GetByte(address);
            }

            if (!this.IsEnabled())
            {
                return this._waveRam.GetByte(address);
            }

            if (this._waveRam.Accepts(this._lastReadAddress) && (this.Gbc || this._ticksSinceRead < 2))
            {
                return this._waveRam.GetByte(this._lastReadAddress);
            }

            return 0xff;
        }


        public override void SetByte(int address, int value)
        {
            if (!this._waveRam.Accepts(address))
            {
                base.SetByte(address, value);
                return;
            }

            if (!this.IsEnabled())
            {
                this._waveRam.SetByte(address, value);
            }
            else if (this._waveRam.Accepts(this._lastReadAddress) && (this.Gbc || this._ticksSinceRead < 2))
            {
                this._waveRam.SetByte(this._lastReadAddress, value);
            }
        }

        protected override void SetNr0(int value)
        {
            base.SetNr0(value);
            this.DacEnabled = (value & (1 << 7)) != 0;
            this.ChannelEnabled &= this.DacEnabled;
        }

        protected override void SetNr1(int value)
        {
            base.SetNr1(value);
            this.Length.SetLength(256 - value);
        }

        protected override void SetNr4(int value)
        {
            if (!this.Gbc && (value & (1 << 7)) != 0)
            {
                if (this.IsEnabled() && this._freqDivider == 2)
                {
                    var pos = this._i / 2;
                    if (pos < 4)
                    {
                        this._waveRam.SetByte(0xff30, this._waveRam.GetByte(0xff30 + pos));
                    }
                    else
                    {
                        pos = pos & ~3;
                        for (var j = 0; j < 4; j++)
                        {
                            this._waveRam.SetByte(0xff30 + j, this._waveRam.GetByte(0xff30 + ((pos + j) % 0x10)));
                        }
                    }
                }
            }

            base.SetNr4(value);
        }

        public override void Start()
        {
            this._i = 0;
            this._buffer = 0;
            if (this.Gbc)
            {
                this.Length.Reset();
            }

            this.Length.Start();
        }

        protected override void Trigger()
        {
            this._i = 0;
            this._freqDivider = 6;
            this._triggered = !this.Gbc;
            if (this.Gbc)
            {
                this.GetWaveEntry();
            }
        }

        public override int Tick()
        {
            this._ticksSinceRead++;
            if (!this.UpdateLength())
            {
                return 0;
            }

            if (!this.DacEnabled)
            {
                return 0;
            }

            if ((this.GetNr0() & (1 << 7)) == 0)
            {
                return 0;
            }

            this._freqDivider--;

            if (this._freqDivider == 0)
            {
                this.ResetFreqDivider();
                if (this._triggered)
                {
                    this._lastOutput = (this._buffer >> 4) & 0x0f;
                    this._triggered = false;
                }
                else
                {
                    this._lastOutput = this.GetWaveEntry();
                }

                this._i = (this._i + 1) % 32;
            }

            return this._lastOutput;
        }

        private int GetVolume() => (this.GetNr2() >> 5) & 0b11;

        private int GetWaveEntry()
        {
            this._ticksSinceRead = 0;
            this._lastReadAddress = 0xff30 + (this._i / 2);
            this._buffer = this._waveRam.GetByte(this._lastReadAddress);

            var b = this._buffer;
            if (this._i % 2 == 0)
            {
                b = (b >> 4) & 0x0f;
            }
            else
            {
                b = b & 0x0f;
            }

            return this.GetVolume() switch
            {
                0 => 0,
                1 => b,
                2 => b >> 1,
                3 => b >> 2,
                _ => throw new InvalidOperationException("Illegal state")
            };
        }

        private void ResetFreqDivider() => this._freqDivider = this.GetFrequency() * 2;
    }
}