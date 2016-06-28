namespace DC_SB.Utils.Players
{
    class WMPlayer : IPlayer
    {
        private WMPLib.WindowsMediaPlayer player;
        public int Volume
        {
            get { return player.settings.volume; }
            set { player.settings.volume = value; }
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

        public void Play(string filePath)
        {
            player.URL = filePath;
            player.controls.play();
        }

        public void SetDevice(int device)
        {
        }
    }
}
