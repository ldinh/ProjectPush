using System.Collections.Generic;
using CygenGames.Sky2D;
using UnityEngine;
using Random = UnityEngine.Random;

/* Changes Log
 * Version 1.0.1
 * - Modified to work with different sizes of sky texture as long as it is a 8:1 ratio width to height
*/

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class Sky : MonoBehaviour
{
    private const float ChangeSensitivity = 0.01f;

    public Camera Camera;
    public Texture2D SkyGradients;
    public bool UseSimpleShader;
    public int WidthSegments = 32;
    public int HeightSegments = 16;
    public float JitterAmount = 0.5f;
    public Color CloudColor = new Color(0.85f, 0.5f, 0.3f, 0.45f);
    public float CloudCover = 0.5f;
    public float SkyIntensity = 1;
    public float WindSpeed = 40;
    public float VerticalMotion = 0.1f;
    public float TimeOfDay = 12;
    public AnimationCurve TimeOfDayRemapCurve;

    private Mesh mesh;
    private Texture2D texture1;
    private Texture2D texture2;
    private Texture2D texture3;
    private readonly List<Vector3> vertices = new List<Vector3>();
    private readonly List<Vector2> uvs = new List<Vector2>();
    private readonly List<int> triangles = new List<int>();
    private int frameWidth;
    private float blendAmount;
    private float lastBlend;
    private float segmentWidth = 1;
    private float segmentHeight = 1;

    public Material SkyMaterial { get; set; }
    public ISkySim SkySim { get; set; }
    public float CloudOffset { get; set; }
    public Color CurrentCloudColor { get; set; }
    public bool IsRunning { get; private set; }

    private Transform _transform;
    public Transform Transform
    {
        get
        {
            if (_transform == null) _transform = transform;
            return _transform;
        }
    }

    public Vector3 Position
    {
        get { return Transform.position; }
        set { Transform.position = value; }
    }

    public Vector2 Size
    {
        get
        {
            var size = Vector2.one;

            if (Camera != null)
            {
                var wp1 = Camera.ViewportToWorldPoint(new Vector3(0, 0, Camera.farClipPlane - 1));
                var wp2 = Camera.ViewportToWorldPoint(new Vector3(1, 1, Camera.farClipPlane - 1));

                size.x = Mathf.Abs(wp2.x - wp1.x);
                size.y = Mathf.Abs(wp2.y - wp1.y);
            }

            return size;
        }
    }

    private void Awake()
    {
        mesh = GetComponent<MeshFilter>().mesh ?? new Mesh();
        SkyMaterial = !UseSimpleShader
                          ? new Material(Resources.Load("Shaders/CG_Sky") as Shader)
                          : new Material(Resources.Load("Shaders/CG_SimpleSky") as Shader);

        SkyMaterial.SetTexture("_CloudTex", SkyGradients);
        GetComponent<Renderer>().sharedMaterial = SkyMaterial;

        frameWidth = SkyGradients.width / 8;
        texture1 = new Texture2D(frameWidth, SkyGradients.height, TextureFormat.RGB24, false);
        texture2 = new Texture2D(frameWidth, SkyGradients.height, TextureFormat.RGB24, false);

        CurrentCloudColor = CloudColor;

        if (TimeOfDayRemapCurve == null) TimeOfDayRemapCurve = AnimationCurve.Linear(0, 0, 24, 24);
    }

    private void Start()
    {
        if (SkyGradients == null)
        {
            Debug.LogError("Select a sky texture");
            return;
        } 
        
        IsRunning = true;
        BuildMesh();
        BakeVertexColors();
        SkyUpdate();
    }

    private void FixedUpdate()
    {
        if (SkyGradients == null) return;

        if (SkySim != null) SkySim.Update();
        SkyUpdate();
    }

    private void SkyUpdate()
    {
        PositionAndScale();

        CloudOffset += Time.fixedDeltaTime * (WindSpeed * 0.001f);

        SkyMaterial.SetColor("_CloudColor", CurrentCloudColor);
        SkyMaterial.SetFloat("_CloudCover", CloudCover);
        SkyMaterial.SetFloat("_SkyIntensity", SkyIntensity);
        SkyMaterial.SetFloat("_CloudOffset", CloudOffset);
        SkyMaterial.SetFloat("_VerticalMotion", VerticalMotion);

        var time = Mathf.Clamp(TimeOfDayRemapCurve.Evaluate(TimeOfDay), 0, 24);
        blendAmount = time / 24f * 7;
        if (Mathf.Abs(lastBlend - blendAmount) > ChangeSensitivity) BakeVertexColors();
    }

    public void Jitter()
    {
        var verts = vertices.ToArray();
        for (var i = 0; i < vertices.Count; i++)
        {
            var x = i % (WidthSegments + 1);
            var y = i / (WidthSegments + 1);
            var v = vertices[i];
            if (x > 0 && x < WidthSegments && y > 0 && y < HeightSegments)
                verts[i] = v + new Vector3(Random.Range(-segmentWidth * 0.5f, segmentWidth * 0.5f),
                                           Random.Range(-segmentHeight * 0.5f, segmentHeight * 0.5f),
                                           0) * JitterAmount;
        }

        mesh.vertices = verts;
    }

    public void BakeVertexColors()
    {
        var colors = new Color[mesh.vertexCount];
        var l = Mathf.FloorToInt(blendAmount) % 7;
        var h = Mathf.CeilToInt(blendAmount) % 7;
        var blend = blendAmount - l;

        try
        {
            texture1.SetPixels(SkyGradients.GetPixels(l * frameWidth, 0, frameWidth, SkyGradients.height));
            texture1.Apply();
            texture2.SetPixels(SkyGradients.GetPixels(h * frameWidth, 0, frameWidth, SkyGradients.height));
            texture2.Apply();

            if (texture3 == null)
            {
                texture3 = new Texture2D(frameWidth, SkyGradients.height, TextureFormat.RGB24, false);
                texture3.SetPixels(SkyGradients.GetPixels(7 * frameWidth, 0, frameWidth, SkyGradients.height));
                texture3.Apply();
                SkyMaterial.SetTexture("_NoiseTex", texture3);
            }
        }
        catch
        {
            Debug.LogError("Make sure your sky gradient texture is readable.");
            return;
        }

        for (var i = 0; i < colors.Length; i++)
        {
            var uv = mesh.uv[i];
            var x = Mathf.FloorToInt(uv.x * (frameWidth - 1));
            var y = Mathf.FloorToInt(uv.y * (texture1.height - 1));

            var c1 = texture1.GetPixel(x, y);
            var c2 = texture2.GetPixel(x + frameWidth, y);

            colors[i] = Color.Lerp(c1, c2, blend);
        }

        mesh.colors = colors;
        lastBlend = blendAmount;
    }

    public void BuildMesh()
    {
        vertices.Clear();
        uvs.Clear();
        triangles.Clear();

        segmentWidth = Size.x / WidthSegments;
        segmentHeight = Size.y / HeightSegments;

        for (var y = 0; y < HeightSegments + 1; y++)
            for (var x = 0; x < WidthSegments + 1; x++)
            {
                var pos = new Vector3(x * segmentWidth, y * segmentHeight, 0) - new Vector3(Size.x / 2f, Size.y / 2f, 0);
                vertices.Add(pos);
                uvs.Add(new Vector2(x / (float)WidthSegments, y / (float)HeightSegments));
            }

        for (var y = 0; y < HeightSegments; y++)
            for (var x = 0; x < WidthSegments; x++)
            {
                var index = (WidthSegments + 1) * y + x;
                triangles.Add(index + WidthSegments + 1);
                triangles.Add(index + 1);
                triangles.Add(index);
                triangles.Add(index + WidthSegments + 1);
                triangles.Add(index + WidthSegments + 2);
                triangles.Add(index + 1);
            }

        mesh.Clear();
        mesh.vertices = vertices.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        Jitter();
    }

    private void PositionAndScale()
    {
        if (Camera == null) return;

        Transform.position = Camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, Camera.farClipPlane - 1));

        Transform.LookAt(Camera.main.transform);
        Transform.localScale = new Vector3(1, Screen.width / (float)Screen.height, 1);
    }

    public void OnDrawGizmos()
    {
        Vector3 v1;
        Vector3 v2;
        Vector3 v3;
        Vector3 v4;

        if (Camera != null)
        {
            var cam = Camera.transform;
            var depth = Camera.farClipPlane - 1;

            var T = System.Type.GetType("UnityEditor.GameView,UnityEditor");
            var getSizeOfMainGameView = T.GetMethod("GetSizeOfMainGameView", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            var size = (Vector2)getSizeOfMainGameView.Invoke(null, null);

            var tan = Mathf.Tan(Camera.fieldOfView * 0.5f * Mathf.Deg2Rad);
            var y = tan * depth;
            var x = y * (size.x / size.y);

            v1 = new Vector3(-x, y, depth);
            v2 = new Vector3(x, y, depth);
            v3 = new Vector3(x, -y, depth);
            v4 = new Vector3(-x, -y, depth);

            Gizmos.matrix = Matrix4x4.TRS(cam.position, cam.rotation, Vector3.one);
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(v1, v2);
            Gizmos.DrawLine(v2, v3);
            Gizmos.DrawLine(v3, v4);
            Gizmos.DrawLine(v4, v1);

            return;
        }

        v1 = new Vector3(-0.5f, 0.5f, 0);
        v2 = new Vector3(0.5f, 0.5f, 0);
        v3 = new Vector3(0.5f, -0.5f, 0);
        v4 = new Vector3(-0.5f, -0.5f, 0);
        Gizmos.matrix = Matrix4x4.TRS(Transform.position, Transform.rotation, Transform.localScale);
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(v1, v2);
        Gizmos.DrawLine(v2, v3);
        Gizmos.DrawLine(v3, v4);
        Gizmos.DrawLine(v4, v1);
    }

    private void OnApplicationQuit()
    {
        IsRunning = false;
    }
}