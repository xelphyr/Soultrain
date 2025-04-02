using UnityEngine;
using UnityEngine.UI;

public class FogObscure : MonoBehaviour
{
    public Image fogOverlay;
    public float fadeSpeed = 2f;

    private Color overlayColor;

    private int numColliders = 0;

    void Start()
    {
        overlayColor = fogOverlay.color;
        overlayColor.a = 0f;
        fogOverlay.color = overlayColor;
    }

    void Update()
    {
        if (numColliders>0)
        {
            overlayColor.a = Mathf.Lerp(overlayColor.a, 0.45f, Time.deltaTime * fadeSpeed);
        }
        else
        {
            overlayColor.a = Mathf.Lerp(overlayColor.a, 0f, Time.deltaTime * fadeSpeed);
        }
        fogOverlay.color = overlayColor;
    }

    public void EnterFog()
    {
        numColliders +=1;

    }

    public void ExitFog()
    {
        numColliders = Mathf.Max(0, numColliders-1);
    }
}
