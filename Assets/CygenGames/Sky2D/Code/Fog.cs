using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class Fog : MonoBehaviour
{
    private const float ChangeSensitivity = 0.01f;

    public Camera Camera;
    public Texture2D FogTexture;
    public Vector2 FogTiling = Vector2.one;
    public Color FogColor = Color.white;
    public float FogCover = 0.5f;
    public float WindSpeed = 40;
    public float VerticalSpeed = 0.1f;
    public float Depth;

    private Mesh mesh;
    private readonly List<Vector3> vertices = new List<Vector3>();
    private readonly List<Vector2> uvs = new List<Vector2>();
    private readonly List<int> triangles = new List<int>();
    private float lastDepth;


    public Material FogMaterial { get; set; }
    public float FogOffset { get; set; }

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
                var wp1 = Camera.ViewportToWorldPoint(new Vector3(0, 0, Depth));
                var wp2 = Camera.ViewportToWorldPoint(new Vector3(1, 1, Depth));

                size.x = Mathf.Abs(wp2.x - wp1.x);
                size.y = Mathf.Abs(wp2.y - wp1.y);
            }

            return size;
        }
    }

    private void Awake()
    {
        mesh = GetComponent<MeshFilter>().mesh ?? new Mesh();
        FogMaterial = new Material(Resources.Load("Shaders/CG_Fog") as Shader);
        FogMaterial.SetTexture("_FogTex", FogTexture);
        GetComponent<Renderer>().sharedMaterial = FogMaterial;
    }

    private void Start()
    {
        if (FogTexture == null)
        {
            Debug.LogError("Select a fog texture");
            return;
        }

        BuildMesh();
        FogUpdate();

        FogOffset = Random.Range(0f, 500f);
    }

    private void FixedUpdate()
    {
        if (FogTexture == null) return;
        FogUpdate();
        lastDepth = Depth;
    }

    private void FogUpdate()
    {
        if (Mathf.Abs(lastDepth - Depth) > 0.1f) BuildMesh();

        PositionAndScale();

        FogOffset += Time.fixedDeltaTime * (WindSpeed * 0.001f);

        FogMaterial.SetColor("_FogColor", FogColor);
        FogMaterial.SetFloat("_FogCover", FogCover);
        FogMaterial.SetFloat("_FogOffset", FogOffset);
        FogMaterial.SetFloat("_FogTilingX", FogTiling.x);
        FogMaterial.SetFloat("_FogTilingY", FogTiling.y);
        FogMaterial.SetFloat("_VerticalMotion", VerticalSpeed);
    }

    public void BuildMesh()
    {
        vertices.Clear();
        uvs.Clear();
        triangles.Clear();
        var s = Size;
        vertices.Add(new Vector3(-0.5f * s.x, 0.5f * s.y, 0));
        vertices.Add(new Vector3(0.5f * s.x, 0.5f * s.y, 0));
        vertices.Add(new Vector3(0.5f * s.x, -0.5f * s.y, 0));
        vertices.Add(new Vector3(-0.5f * s.x, -0.5f * s.y, 0));

        uvs.Add(new Vector2(0, 1));
        uvs.Add(new Vector2(1, 1));
        uvs.Add(new Vector2(1, 0));
        uvs.Add(new Vector2(0, 0));

        triangles.Add(0);
        triangles.Add(1);
        triangles.Add(2);
        triangles.Add(0);
        triangles.Add(2);
        triangles.Add(3);

        mesh.Clear();
        mesh.vertices = vertices.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
    }

    private void PositionAndScale()
    {
        if (Camera != null)
            Transform.position = Camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, Depth));
        else
            Depth = 0;
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
            var T = System.Type.GetType("UnityEditor.GameView,UnityEditor");
            var getSizeOfMainGameView = T.GetMethod("GetSizeOfMainGameView", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            var size = (Vector2)getSizeOfMainGameView.Invoke(null, null);

            var tan = Mathf.Tan(Camera.fieldOfView * 0.5f * Mathf.Deg2Rad);
            var y = tan * Depth;
            var x = y * (size.x / size.y);

            v1 = new Vector3(-x, y, Depth);
            v2 = new Vector3(x, y, Depth);
            v3 = new Vector3(x, -y, Depth);
            v4 = new Vector3(-x, -y, Depth);

            Gizmos.matrix = Matrix4x4.TRS(cam.position, cam.rotation, Vector3.one);
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
        Gizmos.DrawLine(v1, v2);
        Gizmos.DrawLine(v2, v3);
        Gizmos.DrawLine(v3, v4);
        Gizmos.DrawLine(v4, v1);
    }
}