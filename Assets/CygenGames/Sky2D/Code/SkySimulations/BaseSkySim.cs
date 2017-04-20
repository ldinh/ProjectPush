using UnityEngine;

namespace CygenGames.Sky2D
{
    public abstract class BaseSkySim : ISkySim
    {
        public Sky Sky { get; protected set; }
        public bool IsAnimated { get; set; }

        protected BaseSkySim(Sky sky)
        {
            Sky = sky;
        }

        public abstract void Animate();

        public virtual void Update()
        {
            if (IsAnimated) Animate();
        }
    }
}