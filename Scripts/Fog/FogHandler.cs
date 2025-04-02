using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class FogHandler : MonoBehaviour
{
    [Header("Fog Geometry")]
    public float innerRadius = 30f;
    public float outerRadius = 100f;
    public float height = 5f;
    public int segments = 64;

    [Header("Fog Effects")]
    public ParticleSystem borderParticles;
    public FogObscure fogObscure;

    [Header("Fog Collider Settings")]
    public int colliderResolution = 24;
    public float wallThickness = 1f;

    [Header("Damage Settings")]
    public float fogDamage = 10f;
    public float fogTickInterval = 0.1f;

    private bool isUpdating = false;
    private float targetOuterRadius;

    private Dictionary<Transform, float> fogDamageCooldowns = new();
    private List<BoxCollider> fogWalls = new();

    void Start()
    {
        GenerateFogMesh(innerRadius, outerRadius, height, segments);
        GenerateFogWallColliders();
        targetOuterRadius = outerRadius;
    }

    void Update()
    {
        HandleFogScaling();
    }

    public void ExpandFog(float newOuterRadius)
    {
        targetOuterRadius = newOuterRadius;
        isUpdating = true;
    }

    void HandleFogScaling()
    {
        if (!isUpdating) return;

        // Calculate the new radius before assigning it
        float newRadius = Mathf.Lerp(outerRadius, targetOuterRadius, 0.3f);

        // Update colliders *first* based on the in-progress radius
        outerRadius = newRadius;
        UpdateFogWallColliders();

        // Then update the mesh
        GenerateFogMesh(innerRadius, outerRadius, height, segments);

        // Stop updating if close enough
        if (Mathf.Abs(outerRadius - targetOuterRadius) < 0.1f)
        {
            outerRadius = targetOuterRadius;
            isUpdating = false;
        }
    }

    void GenerateFogWallColliders()
    {
        for (int i = 0; i < colliderResolution; i++)
        {
            GameObject wall = new GameObject($"FogWall_{i}");
            wall.transform.parent = transform;
            wall.layer = gameObject.layer;

            BoxCollider collider = wall.AddComponent<BoxCollider>();
            collider.isTrigger = true;

            FogWallTrigger trigger = wall.AddComponent<FogWallTrigger>();
            trigger.handler = this;
            trigger.fogObscure = fogObscure;

            fogWalls.Add(collider);
        }

        UpdateFogWallColliders();
    }

    void UpdateFogWallColliders()
    {
        float angleStep = 360f / colliderResolution;
        float midRadius = (innerRadius + outerRadius) / 2f;
        float fogRingWidth = outerRadius - innerRadius;

        float arcLength = 2f * Mathf.PI * outerRadius / colliderResolution;

        for (int i = 0; i < fogWalls.Count; i++)
        {
            float angleDeg = i * angleStep;
            float angleRad = angleDeg * Mathf.Deg2Rad;

            Vector3 direction = new Vector3(Mathf.Cos(angleRad), 0, Mathf.Sin(angleRad));
            Vector3 position = direction * midRadius;
            position.y = height / 2f;

            Quaternion rotation = Quaternion.LookRotation(direction);

            Transform t = fogWalls[i].transform;
            t.localPosition = position;
            t.localRotation = rotation;

            fogWalls[i].size = new Vector3(arcLength * 1.1f, height, fogRingWidth); // 1.1x = small overlap
        }
    }

    public void TryDamagePlayer(Transform part)
    {
        Transform root = part.root;

        if (!fogDamageCooldowns.TryGetValue(root, out float lastTime))
            lastTime = -999f;

        if (Time.time - lastTime < fogTickInterval) return;

        Health health = root.GetComponent<Health>();
        if (health != null)
        {
            health.TakeDamage(fogDamage * fogTickInterval);
            fogDamageCooldowns[root] = Time.time;
        }
    }

    void GenerateFogMesh(float innerRadius, float outerRadius, float height, int segments)
    {
        Mesh mesh = new Mesh();
        mesh.name = "HollowFogRing";

        int vCount = segments * 8;
        Vector3[] vertices = new Vector3[vCount];
        int[] triangles = new int[segments * 8 * 3];

        float angleStep = 2 * Mathf.PI / segments;
        int vi = 0;
        int ti = 0;

        for (int i = 0; i < segments; i++)
        {
            float angle0 = i * angleStep;
            float angle1 = (i + 1) * angleStep;

            Vector3 i0_top = new(Mathf.Cos(angle0) * innerRadius, height, Mathf.Sin(angle0) * innerRadius);
            Vector3 i1_top = new(Mathf.Cos(angle1) * innerRadius, height, Mathf.Sin(angle1) * innerRadius);
            Vector3 o0_top = new(Mathf.Cos(angle0) * outerRadius, height, Mathf.Sin(angle0) * outerRadius);
            Vector3 o1_top = new(Mathf.Cos(angle1) * outerRadius, height, Mathf.Sin(angle1) * outerRadius);

            Vector3 i0_bot = new(i0_top.x, 0, i0_top.z);
            Vector3 i1_bot = new(i1_top.x, 0, i1_top.z);
            Vector3 o0_bot = new(o0_top.x, 0, o0_top.z);
            Vector3 o1_bot = new(o1_top.x, 0, o1_top.z);

            vertices[vi + 0] = o0_top; vertices[vi + 1] = o1_top;
            vertices[vi + 2] = o0_bot; vertices[vi + 3] = o1_bot;
            vertices[vi + 4] = i0_top; vertices[vi + 5] = i1_top;
            vertices[vi + 6] = i0_bot; vertices[vi + 7] = i1_bot;

            triangles[ti++] = vi + 0; triangles[ti++] = vi + 1; triangles[ti++] = vi + 2;
            triangles[ti++] = vi + 1; triangles[ti++] = vi + 3; triangles[ti++] = vi + 2;
            triangles[ti++] = vi + 4; triangles[ti++] = vi + 6; triangles[ti++] = vi + 5;
            triangles[ti++] = vi + 5; triangles[ti++] = vi + 6; triangles[ti++] = vi + 7;
            triangles[ti++] = vi + 0; triangles[ti++] = vi + 5; triangles[ti++] = vi + 1;
            triangles[ti++] = vi + 0; triangles[ti++] = vi + 4; triangles[ti++] = vi + 5;
            triangles[ti++] = vi + 2; triangles[ti++] = vi + 3; triangles[ti++] = vi + 6;
            triangles[ti++] = vi + 3; triangles[ti++] = vi + 7; triangles[ti++] = vi + 6;

            vi += 8;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        GetComponent<MeshFilter>().mesh = mesh;

        var renderer = GetComponent<MeshRenderer>();
        renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        renderer.receiveShadows = false;

        var sh = borderParticles.shape;
        sh.radius = innerRadius;
    }
}