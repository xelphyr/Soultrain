using UnityEngine;

public class BillboardScript : MonoBehaviour
{
    public Vector3 worldDirectionOffset = new Vector3(0, 2f, 0.5f); // Global direction
    private Transform cam;
    private Transform target;

    private Vector3 lastPosition;
    private Vector3 lastScale;

    void Start()
    {
        cam = Camera.main.transform;
        target = transform.parent;

        lastPosition = target.position;
        lastScale = target.lossyScale;

        UpdatePosition();
    }

    void LateUpdate()
    {
        // Only update if position or scale changed
        if (target.position != lastPosition || target.lossyScale != lastScale)
        {
            UpdatePosition();
            lastPosition = target.position;
            lastScale = target.lossyScale;
        }

        // Always face the camera
        transform.rotation = Quaternion.LookRotation(cam.forward);
    }

    void UpdatePosition()
    {
        // Offset direction stays global, but scaled based on parent scale
        Vector3 scaledOffset = new Vector3(
            worldDirectionOffset.x * target.lossyScale.x,
            worldDirectionOffset.y * target.lossyScale.y,
            worldDirectionOffset.z * target.lossyScale.z
        );

        transform.position = target.position + scaledOffset;
    }
}