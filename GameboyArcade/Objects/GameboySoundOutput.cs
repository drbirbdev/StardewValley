using System;
using System.Threading;
using BirbShared;
using CoreBoy;
using CoreBoy.sound;
using Microsoft.Xna.Framework.Audio;
using StardewValley;

// TODO: not working
namespace GameboyArcade
{
    public delegate void SoundProducedEventHandler(object sender, byte[] soundData);

    class GameboySoundOutput : ISoundOutput
    {
        public const int SAMPLE_RATE = 22050;

        public event SoundProducedEventHandler OnSoundProduced;

        private const int BUFFER_SIZE = 1024;
        private readonly int DIVIDER = Gameboy.TicksPerSec / SAMPLE_RATE;

        private byte[] Buffer;

        private int i;
        private int tick;

        public GameboySoundOutput()
        {
            this.Buffer = new byte[BUFFER_SIZE];
        }

        public void Play(int left, int right)
        {
            if (tick++ != 0)
            {
                tick %= DIVIDER;
                return;
            }

            // TODO: make play signiture provide bytes

            Buffer[this.i++] = (byte)left;
            Buffer[this.i++] = (byte)right;
            if (this.i >= BUFFER_SIZE)
            {
                OnSoundProduced?.Invoke(this, this.Buffer);
                this.i = 0;
                for (int i = 0; i < BUFFER_SIZE; i++)
                {
                    Buffer[i] = 0;
                }
            }
        }

        public void Start()
        {
            Log.Debug("Start Sound");
            /*
            SoundInstance = Effect.CreateInstance();
            SoundInstance.Play();
            */
            Game1.playSound("gameboySound");
            for (int i = 0; i < BUFFER_SIZE; i++)
            {
                Buffer[i] = 0;
            }
        }

        public void Stop()
        {
            Log.Debug("Stop sound called, but ignored");
            /*
            if (SoundInstance is null)
            {
                Log.Debug("Sound wasn't started");
                return;
            }

            SoundInstance.IsLooped = false;
            while (SoundInstance.State == SoundState.Playing)
            {
                Thread.Sleep(1);
            }
            SoundInstance.Dispose();
            SoundInstance = null;
            */
        }


    }
}
