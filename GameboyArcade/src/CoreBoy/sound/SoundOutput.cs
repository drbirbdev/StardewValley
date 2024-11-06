namespace CoreBoy.sound
{
    public interface ISoundOutput
    {
        void Start();
        void Stop();
        void Play(byte left, byte right);
    }
}
