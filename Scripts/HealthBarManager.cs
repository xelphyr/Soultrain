using UnityEngine;
using System.Collections.Generic;

public class HealthBarManager : MonoBehaviour
{
    public static HealthBarManager Instance;

    public GameObject healthBarPrefab;
    public Canvas canvas;  // Screen Space - Camera canvas
    public int poolSize = 20;

    private Queue<HealthBarUI> pool = new Queue<HealthBarUI>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // Preload pool
        for (int i = 0; i < poolSize; i++)
        {
            CreateHealthBar();
        }
    }

    private HealthBarUI CreateHealthBar()
    {
        GameObject obj = Instantiate(healthBarPrefab, canvas.transform);
        obj.SetActive(false);
        var hb = obj.GetComponent<HealthBarUI>();
        pool.Enqueue(hb);
        return hb;
    }

    public HealthBarUI GetHealthBar(Transform target)
    {
        HealthBarUI hb;

        if (pool.Count > 0)
        {
            hb = pool.Dequeue();
        }
        else
        {
            hb = CreateHealthBar();
        }

        hb.gameObject.SetActive(true);
        hb.SetTarget(target);
        return hb;
    }

    public void ReturnHealthBar(HealthBarUI hb)
    {
        hb.Hide();
        pool.Enqueue(hb);
    }
}