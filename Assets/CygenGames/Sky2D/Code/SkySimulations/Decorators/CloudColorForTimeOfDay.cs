using UnityEngine;

namespace CygenGames.Sky2D
{
    public class CloudColorForTimeOfDay : SkySimDecorator
    {
        protected Color CloudColor;

        public CloudColorForTimeOfDay(ISkySim skySim) : base(skySim)
        {
            IsAnimated = true;
        }

        public override void Animate()
        {
            base.Animate();

            var timeOfDay = Sky.TimeOfDay;

            var color = Sky.CloudColor;

            if (timeOfDay < 4)
                color = Color.Lerp(Color.white, color, Mathf.Clamp01(Mathf.Lerp(0, 1, timeOfDay / 4)));
            else if (timeOfDay > 20)
                color = Color.Lerp(Color.white, color, Mathf.Clamp01(Mathf.Lerp(1, 0, (timeOfDay - 20) / 4)));

            if (timeOfDay < 5)
                color.a = Mathf.Clamp01(Mathf.Lerp(0.1f, 0.7f, timeOfDay / 5));
            else if (timeOfDay > 21)
                color.a = Mathf.Clamp01(Mathf.Lerp(0.7f, 0.1f, (timeOfDay - 21) / 3));
            else
                color.a = Mathf.Clamp(Mathf.Abs(timeOfDay - 12) / 3f, 0.3f, 0.7f);
            

            CloudColor = color;
        }

        public override void Update()
        {
            base.Update();
            SkySim.Update();
            Sky.CurrentCloudColor = CloudColor;
        }
    }
}