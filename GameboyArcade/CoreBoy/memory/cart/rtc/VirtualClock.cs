using System;

namespace CoreBoy.memory.cart.rtc
{
    public class VirtualClock : IClock
    {
        private DateTimeOffset _clock = DateTimeOffset.UtcNow;
        public long CurrentTimeMillis() => this._clock.ToUnixTimeMilliseconds();
        public void Forward(TimeSpan time) => this._clock += time;
    }
}