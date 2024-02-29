namespace CoreBoy.sound
{
    public class Lfsr
    {
        public int Value { get; private set; }

        public Lfsr() => this.Reset();
        public void Start() => this.Reset();
        public void Reset() => this.Value = 0x7fff;

        public int NextBit(bool widthMode7)
        {
            bool x = ((this.Value & 1) ^ ((this.Value & 2) >> 1)) != 0;
            this.Value = this.Value >> 1;
            this.Value = this.Value | (x ? (1 << 14) : 0);

            if (widthMode7)
            {
                this.Value = this.Value | (x ? (1 << 6) : 0);
            }

            return 1 & ~this.Value;
        }
    }
}