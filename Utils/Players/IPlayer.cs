namespace DC_SB.Utils
{
    public interface IPlayer
    {
        int Volume { get; set; }
        void Play(string filePath, int volume, bool loop);
        void Pause();
        void Continue();
        void SetDevice(int device);
    }
}
