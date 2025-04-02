using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public Transform target;
    public float heightAbove = 2f;
    float maxVisibleDistance = 120f;
    public Image fillImage;

    private Camera cam;
    private RectTransform rectTransform;
    private RectTransform canvasRect;
    private Image[] allImages;

    void Awake()
    {
        cam = Camera.main;
        rectTransform = GetComponent<RectTransform>();
        canvasRect = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        allImages = GetComponentsInChildren<Image>(true); // gets both background and fill
    }

    void Start()
    {
        if (cam == null)
            cam = Camera.main;
    }

    void LateUpdate()
    {
        cam = Camera.main;
        if (target == null || canvasRect == null || cam == null) return;

        Vector3 worldPos = target.position + Vector3.up * heightAbove;
        Vector3 toTarget = worldPos - cam.transform.position;
        float distance = Vector3.Distance(cam.transform.position, worldPos);

        float dot = Vector3.Dot(cam.transform.forward, toTarget.normalized);
        bool isInFront = dot > 0f;
        bool isVisible = isInFront && distance < maxVisibleDistance;

        SetCulled(!isVisible);

        if (!isVisible) return;

        Vector3 viewportPos = cam.WorldToViewportPoint(worldPos);
        Vector2 canvasPos = new Vector2(
            Mathf.LerpUnclamped(-canvasRect.sizeDelta.x / 2, canvasRect.sizeDelta.x / 2, viewportPos.x),
            Mathf.LerpUnclamped(-canvasRect.sizeDelta.y / 2, canvasRect.sizeDelta.y / 2, viewportPos.y)
        );

        rectTransform.anchoredPosition = canvasPos;
    }

    public void SetHealth(float ratio)
    {
        fillImage.fillAmount = Mathf.Clamp01(ratio);
    }

    public void SetTarget(Transform t)
    {
        target = t;
        SetCulled(false);
    }

    public void Hide()
    {
        target = null;
        SetCulled(true);
    }

    private void SetCulled(bool isCulled)
    {
        float alpha = isCulled ? 0f : 1f;

        foreach (var img in allImages)
        {
            if (img == null) continue;
            Color c = img.color;
            c.a = alpha;
            img.color = c;
        }
    }
}
