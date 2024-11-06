using System;

namespace CoreBoy.memory
{
    public class DmaAddressSpace : IAddressSpace
    {
        private readonly IAddressSpace _addressSpace;

        public DmaAddressSpace(IAddressSpace addressSpace) => this._addressSpace = addressSpace;
        public bool Accepts(int address) => true;
        public void SetByte(int address, int value) => throw new NotImplementedException("Unsupported");

        public int GetByte(int address) =>
            address < 0xe000
                ? this._addressSpace.GetByte(address)
                : this._addressSpace.GetByte(address - 0x2000);
    }
}