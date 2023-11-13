using System;

namespace CoreBoy.sound
{

    public class SoundMode1 : SoundModeBase
    {
        private int _freqDivider;
        private int _lastOutput;
        private int _i;
        private readonly FrequencySweep _frequencySweep;
        private readonly VolumeEnvelope _volumeEnvelope;

        public SoundMode1(bool gbc) : base(0xff10, 64, gbc)
        {
            this._frequencySweep = new FrequencySweep();
            this._volumeEnvelope = new VolumeEnvelope();
        }

        public override void Start()
        {
            this._i = 0;
            if (this.Gbc)
            {
                this.Length.Reset();
            }

            this.Length.Start();
            this._frequencySweep.Start();
            this._volumeEnvelope.Start();
        }

        protected override void Trigger()
        {
            this._i = 0;
            this._freqDivider = 1;
            this._volumeEnvelope.Trigger();
        }

        public override int Tick()
        {
            this._volumeEnvelope.Tick();

            var e = this.UpdateLength();
            e = this.UpdateSweep() && e;
            e = this.DacEnabled && e;
            if (!e)
            {
                return 0;
            }

            if (--this._freqDivider == 0)
            {
                this.ResetFreqDivider();
                this._lastOutput = (this.GetDuty() & (1 << this._i)) >> this._i;
                this._i = (this._i + 1) % 8;
            }

            return this._lastOutput * this._volumeEnvelope.GetVolume();
        }

        protected override void SetNr0(int value)
        {
            base.SetNr0(value);
            this._frequencySweep.SetNr10(value);
        }

        protected override void SetNr1(int value)
        {
            base.SetNr1(value);
            this.Length.SetLength(64 - (value & 0b00111111));
        }

        protected override void SetNr2(int value)
        {
            base.SetNr2(value);
            this._volumeEnvelope.SetNr2(value);
            this.DacEnabled = (value & 0b11111000) != 0;
            this.ChannelEnabled &= this.DacEnabled;
        }

        protected override void SetNr3(int value)
        {
            base.SetNr3(value);
            this._frequencySweep.SetNr13(value);
        }

        protected override void SetNr4(int value)
        {
            base.SetNr4(value);
            this._frequencySweep.SetNr14(value);
        }

        protected override int GetNr3()
        {
            return this._frequencySweep.GetNr13();
        }

        protected override int GetNr4()
        {
            return (base.GetNr4() & 0b11111000) | (this._frequencySweep.GetNr14() & 0b00000111);
        }

        private int GetDuty()
        {
            switch (this.GetNr1() >> 6)
            {
                case 0:
                    return 0b00000001;
                case 1:
                    return 0b10000001;
                case 2:
                    return 0b10000111;
                case 3:
                    return 0b01111110;
                default:
                    throw new InvalidOperationException("Illegal state exception");
            }
        }

        private void ResetFreqDivider()
        {
            this._freqDivider = this.GetFrequency() * 4;
        }

        protected bool UpdateSweep()
        {
            this._frequencySweep.Tick();
            if (this.ChannelEnabled && !this._frequencySweep.IsEnabled())
            {
                this.ChannelEnabled = false;
            }

            return this.ChannelEnabled;
        }
    }

}