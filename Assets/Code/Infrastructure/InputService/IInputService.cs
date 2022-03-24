namespace Code.Infrastructure.InputService
{
    public interface IInputService
    {
        float Pitch { get; }
        float Yaw { get; }
        float Roll { get; }
        bool Boost { get; }
    }
}