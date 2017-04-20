using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Sky))]
public class SkyInspector : Editor
{
    private SerializedObject sky;
    private SerializedProperty camera;
    private SerializedProperty texture;
    private SerializedProperty useSimpleShader;
    private SerializedProperty width;
    private SerializedProperty height;
    private SerializedProperty jitter;
    private SerializedProperty color;
    private SerializedProperty cover;
    private SerializedProperty intensity;
    private SerializedProperty speed;
    private SerializedProperty time;
    private AnimationCurve curve;

    public void OnEnable()
    {
        sky = new SerializedObject(target);
        camera = sky.FindProperty("Camera");
        texture = sky.FindProperty("SkyGradients");
        useSimpleShader = sky.FindProperty("UseSimpleShader");
        width = sky.FindProperty("WidthSegments");
        height = sky.FindProperty("HeightSegments");
        jitter = sky.FindProperty("JitterAmount");
        color = sky.FindProperty("CloudColor");
        cover = sky.FindProperty("CloudCover");
        intensity = sky.FindProperty("SkyIntensity");
        speed = sky.FindProperty("WindSpeed");
        time = sky.FindProperty("TimeOfDay");
        if (target is Sky) curve = ((Sky)target).TimeOfDayRemapCurve;
    }

    public override void OnInspectorGUI()
    {
        if (sky == null) return;

        var c = color.colorValue;

        sky.Update();

        if (!((Sky)target).IsRunning)
        {
            EditorGUI.indentLevel = 1;
            EditorGUILayout.LabelField("Connections:");
            EditorGUI.indentLevel = 2;
            EditorGUILayout.PropertyField(camera);
            EditorGUILayout.PropertyField(texture);
            EditorGUILayout.PropertyField(useSimpleShader);
            EditorGUI.indentLevel = 1;
            EditorGUILayout.LabelField("Geometry:");
            EditorGUI.indentLevel = 2;
            EditorGUILayout.IntSlider(width, 1, 48);
            EditorGUILayout.IntSlider(height, 1, 48);
            EditorGUILayout.Slider(jitter, 0f, 1f);
            EditorGUI.indentLevel = 1;
        }

        EditorGUILayout.LabelField("Effects:");
        EditorGUI.indentLevel = 2;
        EditorGUILayout.PropertyField(color);
        EditorGUILayout.Slider(cover, 0, 1);
        EditorGUILayout.Slider(intensity, 0, 1);
        EditorGUILayout.Slider(speed, -100, 100);
        EditorGUI.indentLevel = 1;
        EditorGUILayout.LabelField("Time:");
        EditorGUI.indentLevel = 2;
        EditorGUILayout.Slider(time, 0, 24);
        ((Sky)target).TimeOfDayRemapCurve = EditorGUILayout.CurveField("Time Remapping", curve);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.Space();
        if (curve == null || curve.keys.Length < 2 || GUILayout.Button("Reset Curve", GUILayout.Width(128))) ResetCurve();
        EditorGUILayout.EndHorizontal();

        sky.ApplyModifiedProperties();

        if (c != color.colorValue)
            ((Sky)target).CurrentCloudColor = color.colorValue;

        if (GUI.changed) EditorUtility.SetDirty(target);
    }

    private void ResetCurve()
    {
        ((Sky)target).TimeOfDayRemapCurve = curve = AnimationCurve.Linear(0, 0, 24, 24);
    }
}
