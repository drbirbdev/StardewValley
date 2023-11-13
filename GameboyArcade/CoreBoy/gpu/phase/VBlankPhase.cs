namespace CoreBoy.gpu.phase
{
    public class VBlankPhase : IGpuPhase
    {
        private int _ticks;

        public VBlankPhase Start()
        {
            this._ticks = 0;
            return this;
        }

        public bool Tick()
        {
            return ++this._ticks < 456;
        }
    }
}