using System;
using UnityEngine;

namespace CygenGames.Sky2D
{
    public class DayNightSkySim : BaseSkySim
    {
        protected float TimeOfDay;
        protected readonly float DayLength;
        protected DateTime LastUpdate;

        public DayNightSkySim(Sky sky, float dayLength, float startTime) : base(sky)
        {
            DayLength = dayLength * 60;
            TimeOfDay = startTime;
            IsAnimated = true;
            var time = (int)(TimeOfDay / 24f * DayLength);
            LastUpdate = DateTime.Now - new TimeSpan(0, 0, 0, time);
        }

        public override void Animate()
        {
            var elapsed = (float)(DateTime.Now - LastUpdate).TotalSeconds;
            if (elapsed >= DayLength) LastUpdate = DateTime.Now;

            TimeOfDay = Mathf.Lerp(0, 24, elapsed / DayLength);
        }

        public override void Update()
        {
            base.Update();

            Sky.TimeOfDay = TimeOfDay;
        }
    }
}