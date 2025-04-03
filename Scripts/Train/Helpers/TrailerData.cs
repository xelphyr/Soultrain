/*using UnityEngine;

[System.Serializable]
public class TrailerData
{
    public TrailerSO baseStats;
    public WeaponData weapon;

    public float health;

    public TrailerData(TrailerSO so)
    {
        Debug.Log("New trailerData being made");
        baseStats = so;
        health = baseStats.stats[Stat.Health];

        if (so.defaultWeapon != null)
        {
            weapon = new WeaponData(so.defaultWeapon);
        }
        Debug.Log("New trailerData made!");
    }
    
    public GameObject MakeObject(Vector3 pos, Quaternion rot, Transform parent)
    {   
        Debug.Log("Generating trailer1");
        GameObject obj = GameObject.Instantiate(baseStats.trailerPrefab, pos, rot, parent);
        Debug.Log("Generating trailer2");
        Transform weaponPoint = obj.transform.GetChild(0);
        Debug.Log("Generating trailer3");
        if (weapon != null && weapon.baseStats != null && weapon.baseStats.turretPrefab != null)
        {
            Debug.Log($"Generating trailer4 from {baseStats.name}");
            weapon.MakeObject(weaponPoint.position, weaponPoint.rotation, weaponPoint, false);
        }

        Debug.Log("Trailer Generated");
        return obj;
    }
}*/