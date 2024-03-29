using System;

namespace CoreBoy.memory
{
    public class ShadowAddressSpace : IAddressSpace
    {
        private readonly IAddressSpace _addressSpace;
        private readonly int _echoStart;
        private readonly int _targetStart;
        private readonly int _length;

        public ShadowAddressSpace(IAddressSpace addressSpace, int echoStart, int targetStart, int length)
        {
            this._addressSpace = addressSpace;
            this._echoStart = echoStart;
            this._targetStart = targetStart;
            this._length = length;
        }

        public bool Accepts(int address) => address >= this._echoStart && address < this._echoStart + this._length;
        public void SetByte(int address, int value) => this._addressSpace.SetByte(this.Translate(address), value);
        public int GetByte(int address) => this._addressSpace.GetByte(this.Translate(address));

        private int Translate(int address) => this.GetRelative(address) + this._targetStart;

        private int GetRelative(int address)
        {
            int i = address - this._echoStart;
            if (i < 0 || i >= this._length)
            {
                throw new ArgumentException();
            }

            return i;
        }
    }
}