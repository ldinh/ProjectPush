using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CygenGames.Sky2D
{
    public class CloudAnimation : SkySimDecorator
    {
        protected readonly float RateOfChange;
        protected readonly float MinWindSpeed;
        protected readonly float MaxWindSpeed;
        protected Color DesiredCloudColor;
        protected float DesiredCloudCover;
        protected float DesiredWindSpeed;
        protected float DesiredCloudCoverBase;
        protected float DesiredCloudCoverSpread;
        protected float CurrentCloudCoverBase;
        protected float CurrentCloudCoverSpread;
        protected DateTime StartTime;
        protected float LastTimeOfDay;

        public CloudAnimation(ISkySim skySim, float rateOfChange, float minWindSpeed, float maxWindSpeed) : base(skySim)
        {
            RateOfChange = rateOfChange;
            MinWindSpeed = minWindSpeed;
            MaxWindSpeed = maxWindSpeed;
            IsAnimated = true;
            GetNewValues();
            StartTime = DateTime.Now;
        }

        public override void Animate()
        {
            base.Animate();

            var elapsed = (float)(DateTime.Now - StartTime).TotalSeconds;
            CurrentCloudCoverBase = Mathf.Lerp(CurrentCloudCoverBase, DesiredCloudCoverBase, 0.01f);
            CurrentCloudCoverSpread = Mathf.Lerp(CurrentCloudCoverSpread, DesiredCloudCoverSpread, 0.01f);
            DesiredCloudCover = Mathf.Clamp01(CurrentCloudCoverBase + Mathf.Sin(elapsed * RateOfChange) * CurrentCloudCoverSpread);

            var timeOfDay = Sky.TimeOfDay;
            if (LastTimeOfDay - timeOfDay > 20) GetNewValues();
            LastTimeOfDay = timeOfDay;
        }

        private void GetNewValues()
        {
            DesiredCloudCoverBase = Random.Range(0.1f, 0.9f);
            DesiredCloudCoverSpread = Random.Range(0.1f, 0.25f);
            DesiredCloudColor = new Color(Random.Range(0.3f, 0.95f), Random.Range(0.3f, 0.85f), Random.Range(0.3f, 0.75f), 1);
            DesiredWindSpeed = Random.Range(MinWindSpeed, MaxWindSpeed) * (Random.Range(1, 100) % 2 == 0 ? -1 : 1);
        }

        public override void Update()
        {
            base.Update();
            SkySim.Update();
            Sky.CurrentCloudColor = Sky.CloudColor = Color.Lerp(Sky.CloudColor, DesiredCloudColor, 0.01f);
            Sky.CloudCover = Mathf.Lerp(Sky.CloudCover, DesiredCloudCover, 0.01f);
            Sky.WindSpeed = Mathf.Lerp(Sky.WindSpeed, DesiredWindSpeed, 0.01f);
        }
    }
}