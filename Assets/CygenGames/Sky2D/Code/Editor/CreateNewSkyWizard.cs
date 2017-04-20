using UnityEditor;
using UnityEngine;

public class CreateNewSkyWizard : EditorWindow
{
    public Camera Camera;
    public Texture2D SkyGradients;
    public bool UseSimpleShader;
    public int WidthSegments = 16;
    public int HeightSegments = 8;
    public float JitterAmount = 0.5f;
    public bool AddStarPlane;
    public Texture2D StarTexture;
    public int StarCount = 50;
    public float MinStarSize = 4f;
    public float MaxStarSize = 16f;
    public float MinStarBrightness = 0.2f;
    public float MaxStarBrightness = 1f;
    public bool AddWeatherSimulation;
    public float DayLengthInMinutes = 1f;
    public float CloudChangeSpeed = 0.23f;
    public float MinWindSpeed = 30;
    public float MaxWindSpeed = 50;
    public float MinStormDuration = 3;
    public float MaxStormDuration = 5;
    public int StormOddsOneIn = 4000;

    [MenuItem("GameObject/Create Other/2D Sky/Sky...")]
    static void Init()
    {
        var window = GetWindow<CreateNewSkyWizard>(true, "Create 2D Sky");
        window.minSize = new Vector2(400, 620);
        window.maxSize = window.minSize;

        if (Selection.activeGameObject != null)
            window.Camera = Selection.activeGameObject.GetComponent<Camera>();

        window.SkyGradients = Resources.Load("Textures/sky") as Texture2D;
        window.StarTexture = Resources.Load("Textures/star") as Texture2D;
    }

    private void OnGUI()
    {
        GUILayout.Label("General Settings");
        EditorGUI.indentLevel = 2;
        Camera = EditorGUILayout.ObjectField("Target Camera", Camera, typeof(Camera), true) as Camera;
        UseSimpleShader = EditorGUILayout.Toggle("Use Simple Shaders", UseSimpleShader);
        GUILayout.Label("Sky Plane Settings");
        WidthSegments = EditorGUILayout.IntSlider("Width Segments", WidthSegments, 1, 48);
        HeightSegments = EditorGUILayout.IntSlider("Width Segments", HeightSegments, 1, 48);
        JitterAmount = EditorGUILayout.Slider("Jitter Amount", JitterAmount, 0, 1);
        SkyGradients = EditorGUILayout.ObjectField("Sky Gradients Texture", SkyGradients, typeof(Texture2D), true) as Texture2D;
        
        GUILayout.Label("Star Plane Settings");
        AddStarPlane = EditorGUILayout.Toggle("Include", AddStarPlane);
        if (AddStarPlane)
        {
            StarCount = EditorGUILayout.IntField("Star Count", StarCount, GUILayout.Width(200));
            MinStarSize = EditorGUILayout.FloatField("Min Size", MinStarSize, GUILayout.Width(200));
            MaxStarSize = EditorGUILayout.FloatField("Max Size", MaxStarSize, GUILayout.Width(200));
            MinStarBrightness = EditorGUILayout.FloatField("Min Brightness", MinStarBrightness, GUILayout.Width(200));
            MaxStarBrightness = EditorGUILayout.FloatField("Max Brightness", MaxStarBrightness, GUILayout.Width(200));
            StarTexture = EditorGUILayout.ObjectField("Star Texture", StarTexture, typeof(Texture2D), true) as Texture2D;
        }

        GUILayout.Label("Weather Simulation Settings");
        AddWeatherSimulation = EditorGUILayout.Toggle("Include", AddWeatherSimulation);
        if (AddWeatherSimulation)
        {
            DayLengthInMinutes = EditorGUILayout.FloatField("Day Length in Minutes", DayLengthInMinutes, GUILayout.Width(200));
            if (DayLengthInMinutes < 1) DayLengthInMinutes = 1;
            CloudChangeSpeed = EditorGUILayout.FloatField("Clud Formation Speed", CloudChangeSpeed, GUILayout.Width(200));
            MinWindSpeed = EditorGUILayout.FloatField("Min Wind Speed", MinWindSpeed, GUILayout.Width(200));
            MaxWindSpeed = EditorGUILayout.FloatField("Max Wind Speed", MaxWindSpeed, GUILayout.Width(200));
            MinStormDuration = EditorGUILayout.FloatField("Min Storm Duration", MinStormDuration, GUILayout.Width(200));
            MaxStormDuration = EditorGUILayout.FloatField("Max Storm Duration", MaxStormDuration, GUILayout.Width(200));
            StormOddsOneIn = EditorGUILayout.IntField("Chance of Storm", StormOddsOneIn, GUILayout.Width(200));
        }

        if (GUI.Button(new Rect(maxSize.x - 70, maxSize.y - 40, 60, 30),  "Create"))
        {
            Create();
            Close();
        }
    }

    private void Create()
    {
        var skyGo = new GameObject("2D Sky");

        var skyPlaneGo = new GameObject("Sky Plane");
        skyPlaneGo.transform.parent = skyGo.transform;

        var sky = skyPlaneGo.AddComponent<Sky>();
        sky.WidthSegments = WidthSegments;
        sky.HeightSegments = HeightSegments;
        sky.JitterAmount = JitterAmount;
        sky.UseSimpleShader = UseSimpleShader;

        if (SkyGradients != null)
            sky.SkyGradients = SkyGradients;

        if (Camera != null)
        {
            skyGo.transform.parent = Camera.transform;
            sky.Camera = Camera;
        }

        if (AddWeatherSimulation)
        {
            var sim = skyPlaneGo.AddComponent<BasicWeatherSimulater>();
            sim.DayLength = DayLengthInMinutes;
            sim.CloudChangeSpeed = CloudChangeSpeed;
            sim.MinWindSpeed = MinWindSpeed;
            sim.MaxWindSpeed = MaxWindSpeed;
            sim.MinStormDuration = MinStormDuration;
            sim.MaxStormDuration = MaxStormDuration;
            sim.StormOddsOneIn = StormOddsOneIn;
        }

        if (!AddStarPlane) return;

        var starPlaneGo = new GameObject("Star Plane");
        starPlaneGo.transform.parent = skyGo.transform;
        var stars = starPlaneGo.AddComponent<Stars>();

        stars.Sky = sky;
        stars.StarCount = StarCount;
        stars.MinSize = MinStarSize;
        stars.MaxSize = MaxStarSize;
        stars.MinBrightness = MinStarBrightness;
        stars.MaxBrightness = MaxStarBrightness;

        if (StarTexture != null)
            stars.StarTexture = StarTexture;
    }
}