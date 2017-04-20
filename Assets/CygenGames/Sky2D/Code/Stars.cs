using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class Stars : MonoBehaviour
{
    public Sky Sky;
    public Texture2D StarTexture;
    public int StarCount = 50;
    public float MinSize = 1f;
    public float MaxSize = 10f;
    public float MinBrightness = 0.2f;
    public float MaxBrightness = 1f;
    public float RotationSpeed = 6f;

    private Transform myTransform;
    private Mesh mesh;
    private Material starMaterial;
    private readonly List<Vector3> vertices = new List<Vector3>();
    private readonly List<Vector2> uvs = new List<Vector2>();
    private readonly List<int> triangles = new List<int>();
    private readonly List<Color> colors = new List<Color>();
    private bool isInitialized;

    private void FixedUpdate()
    {
        if (Sky == null || StarTexture == null) return;
        if (!isInitialized) Init();

        myTransform.position = Sky.Position + Vector3.back + Vector3.down * Sky.Size.y * 0.5f;

        var timeOfDay = Sky.TimeOfDay;
        var brightness = 0f;
        if (timeOfDay < 6)
            brightness = Mathf.Clamp01(Mathf.Lerp(1, 0, timeOfDay / 6));
        else if (timeOfDay > 18)
            brightness = Mathf.Clamp01(Mathf.Lerp(0, 1, (timeOfDay - 18) / 6));

        starMaterial.SetFloat("_Brightness", brightness);

        myTransform.Rotate(0, 0, Time.fixedDeltaTime * RotationSpeed);
    }

    private void Init()
    {
        myTransform = transform;
        if (StarTexture == null)
        {
            Debug.LogError("Select a star texture");
            return;
        }

        if (Sky == null)
        {
            Debug.LogError("Select a sky object");
            return;
        }

        mesh = GetComponent<MeshFilter>().mesh ?? new Mesh();
        starMaterial = new Material(Resources.Load("Shaders/CG_Star") as Shader);
        starMaterial.SetTexture("_StarTex", StarTexture);
        GetComponent<Renderer>().sharedMaterial = starMaterial;

        BuildMesh();

        isInitialized = true;
    }

    public void BuildMesh()
    {
        vertices.Clear();
        uvs.Clear();
        triangles.Clear();
        colors.Clear();

        //var size = Sky.Size / 2f;
        var w = Sky.Size.x * 0.5f;
        var h = Sky.Size.y;
        var radius = Mathf.Sqrt(w * w + h * h);
        var index = 0;
        for (var i = 0; i < StarCount; i++)
        {
            //var x = Random.Range(-size.x, size.x - MaxSize);
            //var y = Random.Range(-size.y, size.y - MaxSize);
            var pos = Random.insideUnitCircle* radius;
            var s = Random.Range(MinSize, MaxSize) * 2;

            vertices.Add(new Vector3(pos.x, pos.y + s, 0));
            vertices.Add(new Vector3(pos.x + s, pos.y + s, 0));
            vertices.Add(new Vector3(pos.x + s, pos.y, 0));
            vertices.Add(new Vector3(pos.x, pos.y, 0));

            uvs.Add(new Vector2(0, 1));
            uvs.Add(new Vector2(1, 1));
            uvs.Add(new Vector2(1, 0));
            uvs.Add(new Vector2(0, 0));

            triangles.Add(index + 0);
            triangles.Add(index + 1);
            triangles.Add(index + 2);
            triangles.Add(index + 0);
            triangles.Add(index + 2);
            triangles.Add(index + 3);

            var a = Random.Range(MinBrightness, MaxBrightness);
            for (var j = 0; j < 4; j++)
            {
                var r = Random.Range(0.65f, 1f);
                var g = Random.Range(0.65f, 1f);
                colors.Add(new Color(r, g, 1, a));
            }

            index += 4;
        }

        mesh.vertices = vertices.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.colors = colors.ToArray();
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
    }
}