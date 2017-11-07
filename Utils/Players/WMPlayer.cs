namespace DC_SB.Utils.Players
{
    class WMPlayer : IPlayer
    {
        private WMPLib.WindowsMediaPlayer player;
        private float volumeMultiplyer = 1f;
        private int volume;
        public int Volume
        {
            get { return volume; }
            set
            {
                volume = value;
                player.settings.volume = (int)(volume * volumeMultiplyer);
            }
        }

        public WMPlayer()
        {
            player = new WMPLib.WindowsMediaPlayer();
        }

        public void Continue()
        {
            if (player.controls.currentPosition != 0)
            {
                player.controls.play();
            }
        }

        public void Pause()
        {
            player.controls.pause();
        }

        public void Play(string filePath, int volume, bool loop)
        {
            player.URL = filePath;
            volumeMultiplyer = volume / 100f;
            player.settings.volume = (int) (this.volume * volumeMultiplyer);
            player.settings.setMode("loop", loop);
            player.controls.play();
        }

        public void SetDevice(int device)
        {
        }
    }
}
