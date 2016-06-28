using NAudio.Wave;

namespace DC_SB.Utils.Players
{
    class NAudioPlayer : IPlayer
    {
        private WaveOut player;
        private AudioFileReader reader;
        private float volume;
        private int device = 0;
        public int Volume
        {
            get { return (int)(volume * 100); }
            set
            {
                volume = ((float)value) / 100f;
                player.Volume = volume;
            }
        }

        public NAudioPlayer()
        {
            player = new WaveOut();
        }

        public void Continue()
        {
            try { player.Play(); }
            catch (System.NullReferenceException) { }
        }

        public void Pause()
        {
            player.Pause();
        }

        public void Play(string filePath)
        {
            player.Stop();
            player.Dispose();
            player = new WaveOut();
            player.DeviceNumber = device;
            player.Volume = volume;
            if (reader != null) reader.Dispose();

            if (System.IO.File.Exists(filePath))
            {
                try
                {
                    reader = new AudioFileReader(filePath);
                    player.Init(reader);
                    player.Play();
                }
                catch (NAudio.MmException)
                {
                    ErrorHandler.Raise("Cannot play sound. File {0} is unsupported by NAudio.", filePath);
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
