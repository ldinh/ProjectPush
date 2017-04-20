using UnityEngine;
using Random = UnityEngine.Random;

namespace CygenGames.Sky2D
{
    public class StormAnimation : SkySimDecorator
    {
        protected readonly float MinStomrDuration;
        protected readonly float MaxStomrDuration;
        protected readonly int StormOddsOneIn = 4000;
        protected float StartingCloudCover;
        protected float DesiredCloudCover;
        protected float DesiredSkyIntensity;
        protected float StartTime;
        protected float Duration;
        protected bool IsStorming;
        private bool r;

        public StormAnimation(ISkySim skySim, float minStomrDuration, float maxStomrDuration, int stormOddsOneIn)
            : base(skySim)
        {
            MinStomrDuration = minStomrDuration;
            MaxStomrDuration = maxStomrDuration;
            StormOddsOneIn = stormOddsOneIn;
            IsAnimated = true;
        }

        public override void Animate()
        {
            base.Animate();

            var elapsed = Sky.TimeOfDay - StartTime;

            r = Random.Range(0, StormOddsOneIn) == 1;
            if (!IsStorming && r && Sky.TimeOfDay > 7 && Sky.TimeOfDay < 15)
            {
                IsStorming = true;
                StartingCloudCover = Sky.CloudCover;
                DesiredCloudCover = 1f;
                DesiredSkyIntensity = Random.Range(0, 0.6f);
                Duration = Random.Range(MinStomrDuration, MaxStomrDuration);
                StartTime = Sky.TimeOfDay;
                return;
            }
            
            if (IsStorming && elapsed > Duration)
            {
                IsStorming = false;
            }
            else if (IsStorming)
            {
                if (elapsed < 0.5f)
                {
                    Sky.CloudCover = Mathf.Lerp(StartingCloudCover, DesiredCloudCover, elapsed / 0.5f);
                    Sky.SkyIntensity = Mathf.Lerp(1, DesiredSkyIntensity, elapsed / 0.5f);
                }
                else if (elapsed > Duration - 0.5f)
                {
                    Sky.CloudCover = Mathf.Lerp(StartingCloudCover, DesiredCloudCover, (Duration - elapsed) / 0.5f);
                    Sky.SkyIntensity = Mathf.Lerp(1, DesiredSkyIntensity, (Duration - elapsed) / 0.5f);
                }
                else
                {
                    Sky.CloudCover = DesiredCloudCover;
                    Sky.SkyIntensity = DesiredSkyIntensity;
                }

                return;
            }

            DesiredCloudCover = StartingCloudCover;
            DesiredSkyIntensity = 1;
        }

        public override void Update()
        {
            base.Update();
            SkySim.Update();
        }
    }
}