using NAudio.Vorbis;
using NAudio.Wave;
using System;

namespace DC_SB.Utils.Players
{
    class NAudioPlayer : IPlayer
    {
        private WaveOut player;
        private WaveStream reader;
        private float volumeMultiplyer = 1f;
        private float volume;
        private int device = 0;
        public int Volume
        {
            get { return (int) (volume * 100); }
            set
            {
                volume = ((value) / 100f);
                player.Volume = volume * volumeMultiplyer;
            }
        }

        public NAudioPlayer()
        {
            player = new WaveOut();
        }

        public void Continue()
        {
            try { player.Play(); }
            catch (NullReferenceException) { }
        }

        public void Pause()
        {
            player.Pause();
        }

        public void Play(string filePath, int volume, bool loop)
        {
            player.Stop();
            player.Dispose();
            player = new WaveOut();
            player.DeviceNumber = device;
            volumeMultiplyer = volume / 100f;
            player.Volume = this.volume * volumeMultiplyer;
            if (reader != null)
            {
                try { reader.Dispose(); }
                catch { }
            }

            if (System.IO.File.Exists(filePath))
            {
                try
                {
                    try { reader = new AudioFileReader(filePath); }
                    catch { reader = new VorbisWaveReader(filePath); }

                    if (loop)
                        reader = new LoopStream(reader);
                    
                    player.Init(reader);
                    player.Play();
                }
                catch (Exception e)
                {
                    ErrorHandler.Raise("Cannot play sound. File {0} is unsupported by NAudio.\n{1}", filePath, e.Message);
                }
            }
        }

        public void SetDevice(int device)
        {
            this.device = device;
            player.DeviceNumber = device;
        }
    }
}
