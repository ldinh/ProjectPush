using CygenGames.Sky2D;
using UnityEngine;

[RequireComponent(typeof(Sky))]
public class BasicWeatherSimulater : MonoBehaviour
{
    public float DayLength = 1f;
    public float CloudChangeSpeed = 0.23f;
    public float MinWindSpeed = 30;
    public float MaxWindSpeed = 50;
    public float MinStormDuration = 3;
    public float MaxStormDuration = 5;
    public int StormOddsOneIn = 4000;

    private Sky sky;

    public ISkySim Sim { get; private set; }

    private void Awake()
    {
    }

    private void Start()
    {
        sky = GetComponent<Sky>();

        Sim = new DayNightSkySim(sky, DayLength, sky.TimeOfDay);
        //Sim = new CloudAnimation(Sim, CloudChangeSpeed, MinWindSpeed, MaxWindSpeed);
        //Sim = new CloudColorForTimeOfDay(Sim);
        //Sim = new CloudSaturationForTimeOfDay(Sim);
        //Sim = new StormAnimation(Sim, MinStormDuration, MaxStormDuration, StormOddsOneIn);

        sky.SkySim = Sim;
    }
}