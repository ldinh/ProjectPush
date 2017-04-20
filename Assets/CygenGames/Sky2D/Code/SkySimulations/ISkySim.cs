namespace CygenGames.Sky2D
{
    public interface ISkySim
    {
        Sky Sky { get; }
        bool IsAnimated { get; set; }

        void Animate();
        void Update();
    }
}
