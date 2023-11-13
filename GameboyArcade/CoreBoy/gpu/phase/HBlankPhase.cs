namespace CoreBoy.gpu.phase
{
    public class HBlankPhase : IGpuPhase
    {

        private int _ticks;

        public HBlankPhase Start(int ticksInLine)
        {
            this._ticks = ticksInLine;
            return this;
        }

        public bool Tick()
        {
            this._ticks++;
            return this._ticks < 456;
        }

    }
}