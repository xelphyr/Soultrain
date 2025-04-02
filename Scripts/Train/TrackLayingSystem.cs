// TrackLayingSystem (Optimized Version)
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class TrackLayingSystem : MonoBehaviour
{
    [Header("References")]
    public Transform generatorPoint;
    public GameObject railSegmentPrefab;
    public GameObject sleeperPrefab;

    [Header("Track Settings")]
    public float railOffset = 0.75f;
    public float timeBetweenRailSegments = 0.2f;
    public int segmentsBeforeFragment = 5;
    public float sleeperInterval = 1.0f;
    public float segmentMaxHealth = 10f;

    private float timeSinceLastSegment = 0f;
    private float sleeperTimer = 0f;

    private List<Vector3> leftPoints = new();
    private List<Vector3> rightPoints = new();
    private int segmentCounter = -1;

    private List<GameObject> segmentObjects = new();

    // Optimization Buffers & Pools
    private readonly List<CombineInstance> combineInstances = new();
    private CombineInstance[] combineBuffer;
    private readonly List<GameObject> tempToRecycle = new();
    private readonly Queue<GameObject> sleeperPool = new();
    private readonly Queue<GameObject> railPool = new();

    void Update()
    {
        timeSinceLastSegment += Time.deltaTime;
        sleeperTimer += Time.deltaTime;

        if (timeSinceLastSegment >= timeBetweenRailSegments)
        {
            timeSinceLastSegment = 0f;

            Vector3 genPointWorld = generatorPoint.position;
            Vector3 right = generatorPoint.right;

            Vector3 leftPos = genPointWorld - right * railOffset;
            Vector3 rightPos = genPointWorld + right * railOffset;

            leftPoints.Add(leftPos);
            rightPoints.Add(rightPos);
            segmentCounter++;

            if (sleeperTimer >= sleeperInterval)
            {
                sleeperTimer = 0f;
                SpawnSleeper(leftPos, rightPos);
            }

            if (segmentCounter >= segmentsBeforeFragment)
            {
                SpawnRailFragment();
            }
        }
    }

    void SpawnSleeper(Vector3 left, Vector3 right)
    {
        Vector3 center = (left + right) / 2f;
        Quaternion rot = Quaternion.LookRotation(Vector3.Cross(left - right, Vector3.up), Vector3.up);

        GameObject sleeper = GetPooledSleeper(center, rot);
        segmentObjects.Add(sleeper);
    }

    void SpawnRailFragment()
    {
        if (leftPoints.Count < 2 || rightPoints.Count < 2) return;
        StartCoroutine(DeferredCombineTrack());
    }

    IEnumerator DeferredCombineTrack()
    {
        yield return null;

        combineInstances.Clear();
        tempToRecycle.Clear();

        // Copy the current sleepers into a local list and clear the global list.
        List<GameObject> sleepersToMerge = new List<GameObject>(segmentObjects);
        segmentObjects.Clear();

        // Process rail segments as before.
        for (int i = 0; i < leftPoints.Count - 1; i++)
        {
            Vector3 l0 = leftPoints[i];
            Vector3 l1 = leftPoints[i + 1];
            Vector3 r0 = rightPoints[i];
            Vector3 r1 = rightPoints[i + 1];

            Vector3 leftCenter = (l0 + l1) / 2;
            Quaternion leftRot = Quaternion.LookRotation(l1 - l0, Vector3.up);
            float leftScale = (l1 - l0).magnitude;

            GameObject tempLeft = GetPooledRail(leftCenter, leftRot, leftScale);
            tempToRecycle.Add(tempLeft);

            MeshFilter mfLeft = tempLeft.GetComponentInChildren<MeshFilter>();
            if (!mfLeft || !mfLeft.sharedMesh)
                Debug.LogWarning("Missing mesh on rail prefab!");

            if (mfLeft && mfLeft.sharedMesh != null)
            {
                combineInstances.Add(new CombineInstance
                {
                    mesh = mfLeft.sharedMesh,
                    transform = mfLeft.transform.localToWorldMatrix
                });
            }

            Vector3 rightCenter = (r0 + r1) / 2;
            Quaternion rightRot = Quaternion.LookRotation(r1 - r0, Vector3.up);
            float rightScale = (r1 - r0).magnitude;

            GameObject tempRight = GetPooledRail(rightCenter, rightRot, rightScale);
            tempToRecycle.Add(tempRight);

            MeshFilter mfRight = tempRight.GetComponentInChildren<MeshFilter>();
            if (mfRight && mfRight.sharedMesh != null)
            {
                combineInstances.Add(new CombineInstance
                {
                    mesh = mfRight.sharedMesh,
                    transform = mfRight.transform.localToWorldMatrix
                });
            }
        }

        // Now process the sleepers that were captured at the start.
        foreach (GameObject sleeperObj in sleepersToMerge)
        {
            if (sleeperObj.activeInHierarchy)
            {
                MeshFilter mf = sleeperObj.GetComponentInChildren<MeshFilter>();
                if (mf && mf.sharedMesh != null)
                {
                    combineInstances.Add(new CombineInstance
                    {
                        mesh = mf.sharedMesh,
                        transform = mf.transform.localToWorldMatrix
                    });
                }

                tempToRecycle.Add(sleeperObj);
            }
        }

        CombineInstance[] combineBuffer = combineInstances.ToArray();

        Mesh combinedMesh = new Mesh();
        combinedMesh.CombineMeshes(combineBuffer, true, true);

        GameObject combinedSegment = new GameObject("TrackSegment");
        combinedSegment.tag = "Segment";
        combinedSegment.layer = 9;
        combinedSegment.isStatic = true;

        MeshFilter mfCombined = combinedSegment.AddComponent<MeshFilter>();
        mfCombined.mesh = combinedMesh;

        MeshRenderer mrCombined = combinedSegment.AddComponent<MeshRenderer>();
        mrCombined.sharedMaterial = railSegmentPrefab.GetComponentInChildren<MeshRenderer>().sharedMaterial;

        MeshCollider meshCollider = combinedSegment.AddComponent<MeshCollider>();
        meshCollider.sharedMesh = combinedMesh;
        meshCollider.convex = false;
        meshCollider.isTrigger = false;

        Health segmentHealth = combinedSegment.AddComponent<Health>();
        segmentHealth.maxHealth = segmentMaxHealth;

        foreach (var obj in tempToRecycle)
        {
            obj.SetActive(false);
            if (obj.CompareTag("Sleeper"))
                sleeperPool.Enqueue(obj);
            else
                railPool.Enqueue(obj);
        }

        // Reset for next fragment.
        Vector3 tempLeftPoint = leftPoints[^1];
        Vector3 tempRightPoint = rightPoints[^1];

        leftPoints.Clear();
        rightPoints.Clear();

        leftPoints.Add(tempLeftPoint);
        rightPoints.Add(tempRightPoint);

        segmentCounter = 0;
    }

    GameObject GetPooledRail(Vector3 pos, Quaternion rot, float zScale)
    {
        if (!railSegmentPrefab)
        {
            Debug.LogError("🚨 railSegmentPrefab is NOT ASSIGNED.");
            return null;
        }

        GameObject obj = railPool.Count > 0 ? railPool.Dequeue() : Instantiate(railSegmentPrefab);
        if (!obj)
        {
            Debug.LogError("🚨 Instantiated rail prefab is NULL.");
            return null;
        }

        obj.transform.SetPositionAndRotation(pos, rot);
        obj.transform.localScale = new Vector3(1, 1, zScale);
        obj.SetActive(true);
        return obj;
    }

    GameObject GetPooledSleeper(Vector3 pos, Quaternion rot)
    {
        GameObject obj = sleeperPool.Count > 0 ? sleeperPool.Dequeue() : Instantiate(sleeperPrefab);
        obj.transform.SetPositionAndRotation(pos, rot);
        obj.SetActive(true);
        obj.tag = "Sleeper";
        return obj;
    }
}
