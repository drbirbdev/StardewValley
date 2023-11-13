using System.Collections.Generic;
using System.Linq;
using CoreBoy.cpu.op;

namespace CoreBoy.cpu.opcode
{
    public class Opcode
    {
        public int Value { get; }
        public string Label { get; }
        public List<Op> Ops { get; }
        public int Length { get; }

        public Opcode(OpcodeBuilder builder)
        {
            this.Value = builder.GetOpcode();
            this.Label = builder.GetLabel();
            this.Ops = builder.GetOps();
            this.Length = this.Ops.Count <= 0 ? 0 : this.Ops.Max(o => o.OperandLength());
        }

        public override string ToString() => $"{this.Value:X2} {this.Label}";
    }
}