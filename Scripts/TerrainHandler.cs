using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TerrainHandler : MonoBehaviour
{
    [System.Serializable]
    public class Asset
    {
        public GameObject obj;
        public float frequency;
        public Asset(GameObject o, float freq)
        {
            obj = o;
            frequency = freq;
        }
    }
    [SerializeField] public List<Asset> assets = new List<Asset>();
    public int totalAmount;

    float total;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i<totalAmount; i++)
        {
            Vector3 pos;
            do
            {
                pos = new Vector3(
                    Random.Range(
                        transform.position.x - transform.lossyScale.x/2, 
                        transform.position.x + transform.lossyScale.x/2
                    ),
                    transform.position.y + transform.lossyScale.y/2,
                    Random.Range(
                        transform.position.z - transform.lossyScale.z/2, 
                        transform.position.z + transform.lossyScale.z/2
                    )
                );
            }
            while (pos != null && pos.magnitude > transform.lossyScale.x/2);

            GameObject toBeInstantiated = GetWeightedRandom();
            GameObject temp = Instantiate(toBeInstantiated, pos, Quaternion.Euler(-90f, Random.Range(-180f, 180f), 0));
            temp.transform.SetParent(transform, true);
        } 
    }

    public GameObject GetWeightedRandom()
    {
        if (total==0f)
        {
            foreach (var asset in assets)
            {
                total += asset.frequency;
            }
        }
        float chosen = Random.Range(0, total);
        float tempIncrement = 0f;
        for(int i = 0; i<assets.Count; i++)
        {
            tempIncrement += assets[i].frequency;
            if (tempIncrement > chosen) return assets[i].obj;
        }
        return assets[assets.Count-1].obj;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
