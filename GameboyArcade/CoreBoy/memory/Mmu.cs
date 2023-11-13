using System.Collections.Generic;

namespace CoreBoy.memory
{
    public class Mmu : IAddressSpace
    {
        private static readonly IAddressSpace Void = new VoidAddressSpace();
        private readonly List<IAddressSpace> _spaces = new List<IAddressSpace>();

        public void AddAddressSpace(IAddressSpace space) => this._spaces.Add(space);
        public bool Accepts(int address) => true;
        public void SetByte(int address, int value) => this.GetSpace(address).SetByte(address, value);
        public int GetByte(int address) => this.GetSpace(address).GetByte(address);

        private IAddressSpace GetSpace(int address)
        {
            foreach (var s in this._spaces)
            {
                if (s.Accepts(address))
                {
                    return s;
                }
            }

            return Void;
        }

    }
}