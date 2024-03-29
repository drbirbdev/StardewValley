namespace CoreBoy.controller
{
    public class Button
    {
        public static readonly Button Right = new Button(0x01, 0x10);
        public static readonly Button Left = new Button(0x02, 0x10);
        public static readonly Button Up = new Button(0x04, 0x10);
        public static readonly Button Down = new Button(0x08, 0x10);
        public static readonly Button A = new Button(0x01, 0x20);
        public static readonly Button B = new Button(0x02, 0x20);
        public static readonly Button Select = new Button(0x04, 0x20);
        public static readonly Button Start = new Button(0x08, 0x20);

        public int Mask { get; }
        public int Line { get; }

        public Button(int mask, int line)
        {
            this.Mask = mask;
            this.Line = line;
        }
    }
}
