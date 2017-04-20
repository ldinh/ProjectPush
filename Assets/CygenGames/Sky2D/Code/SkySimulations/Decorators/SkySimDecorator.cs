namespace CygenGames.Sky2D
{
    public abstract class SkySimDecorator : BaseSkySim
    {
        protected ISkySim SkySim { get; set; }

        protected SkySimDecorator(ISkySim skySim) : base(skySim.Sky)
        {
            SkySim = skySim;
        }

        public override void Animate()
        {
            SkySim.Animate();
        }

        public override void Update()
        {
            base.Update();
            SkySim.Update();
        }
    }
}