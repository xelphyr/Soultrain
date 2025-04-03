using UnityEngine;

[CreateAssetMenu(fileName = "New Trailer", menuName = "Trailer")]
public class TrailerSO : ScriptableObject
{
    public string trailerName;
    [TextArea] public string description;

    [Header("Settings")]
    public bool isEngine;
    public Stats stats;
    
    [Header("Files")]
    public GameObject trailerPrefab;
    
    // Optional visuals
    public Sprite trailerSprite;

    public TurretSO turret = null;
    
    public GameObject Spawn(Vector3 pos, Quaternion rot, Transform parent)
    {   
        GameObject obj = GameObject.Instantiate(trailerPrefab, pos, rot, parent);
        obj.GetComponent<TrailerTurretSelector>().ActiveTurret = turret;
        Debug.Log($"Trailer Generated: {obj.name}");
        return obj;
    }
    
}