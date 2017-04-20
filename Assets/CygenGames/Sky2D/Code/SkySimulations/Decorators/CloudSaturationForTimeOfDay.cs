using UnityEngine;

namespace CygenGames.Sky2D
{
    public class CloudSaturationForTimeOfDay : SkySimDecorator
    {
        protected float CloudSaturation;

        public CloudSaturationForTimeOfDay(ISkySim skySim) : base(skySim)
        {
            IsAnimated = true;
        }

        public override void Animate()
        {
            base.Animate();

            var timeOfDay = Sky.TimeOfDay;

            if (timeOfDay < 2)
                CloudSaturation = Mathf.Lerp(1, 0, timeOfDay / 2);
            else if (timeOfDay >= 2 && timeOfDay <= 7)
                CloudSaturation = 0;
            else if (timeOfDay > 7 && timeOfDay < 10)
                CloudSaturation = Mathf.Lerp(0, 1, (timeOfDay - 7) / 3f);
            else if (timeOfDay >= 10 && timeOfDay <= 15)
                CloudSaturation = 1;
            else if (timeOfDay > 15 && timeOfDay < 16f)
                CloudSaturation = Mathf.Lerp(1, 0, timeOfDay - 15);
            else if (timeOfDay >= 16 && timeOfDay <= 22)
                CloudSaturation = 0;
            else if (timeOfDay > 22)
                CloudSaturation = Mathf.Lerp(0, 1, (timeOfDay - 22) / 2);

            CloudSaturation *= 0.75f;
        }

        public override void Update()
        {
            base.Update();
            SkySim.Update();
            Sky.CurrentCloudColor = Desaturate(Sky.CurrentCloudColor, CloudSaturation);
        }

        private static Color Desaturate(Color color, float k)
        {
            var intensity = 0.3f * color.r + 0.59f * color.g + 0.11f * color.b;
            color.r = intensity * k + color.r * (1 - k);
            color.g = intensity * k + color.g * (1 - k);
            color.b = intensity * k + color.b * (1 - k);

            return color;
        }
    }
}