using UnityEditor;
using UnityEngine;

public class CreateNewFogWizard : EditorWindow
{
    public Camera Camera;
    public Texture2D FogTexture;
    public Color FogColor = new Color(0.75f, 0.75f, 0.75f, 0.75f);
    public float FogCover = 0.75f;
    public float WindSpeed = 40f;
    public float VerticalSpeed = 0.2f;
    public float Depth = 10f;

    [MenuItem("GameObject/Create Other/2D Sky/Fog...")]
    static void Init()
    {
        var window = GetWindow<CreateNewFogWizard>(true, "Create 2D Fog");
        window.minSize = new Vector2(400, 260);
        window.maxSize = window.minSize;

        if (Selection.activeGameObject != null)
            window.Camera = Selection.activeGameObject.GetComponent<Camera>();

        window.FogTexture = Resources.Load("Textures/sky") as Texture2D;
    }

    private void OnGUI()
    {
        GUILayout.Label("General Settings");
        EditorGUI.indentLevel = 2;
        Camera = EditorGUILayout.ObjectField("Target Camera", Camera, typeof(Camera), true) as Camera;
        FogTexture = EditorGUILayout.ObjectField("Fog Texture(Alpha)", FogTexture, typeof(Texture2D), true) as Texture2D;
        FogColor = EditorGUILayout.ColorField("Fog Color", FogColor);
        FogCover = EditorGUILayout.FloatField("Fog Cover", FogCover);
        WindSpeed = EditorGUILayout.Slider("Wind Speed", WindSpeed, -100f, 100f);
        VerticalSpeed = EditorGUILayout.Slider("Vertical Speed", VerticalSpeed, -5f, 5f);

        if (Camera != null)
            Depth = EditorGUILayout.Slider("Depth", Depth, Camera.nearClipPlane, Camera.farClipPlane);

        if (GUI.Button(new Rect(maxSize.x - 70, maxSize.y - 40, 60, 30),  "Create"))
        {
            Create();
            Close();
        }
    }

    private void Create()
    {
        var fogGo = new GameObject("Fog Plane");

        var fog = fogGo.AddComponent<Fog>();
        fog.FogColor = FogColor;
        fog.FogCover = FogCover;
        fog.WindSpeed = WindSpeed;
        fog.VerticalSpeed = VerticalSpeed;
        fog.Depth = Depth;

        if (FogTexture != null)
            fog.FogTexture = FogTexture;

        if (Camera != null)
        {
            fogGo.transform.parent = Camera.transform;
            fog.Camera = Camera;
        }
    }
}