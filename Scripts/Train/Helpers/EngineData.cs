using UnityEngine;

[System.Serializable]
public class EngineData
{
    public EngineSO baseStats;
    public float maxHealth;
    public float baseStorage;
    public WeaponData weapon;
    
    public EngineData(EngineSO so)
    {
        Debug.Log("New engineData being made");
        baseStats = so;
        maxHealth = so.baseHealth;
        baseStorage = so.baseStorage;
        if (so.defaultWeapon != null)
        {
            weapon = new WeaponData(so.defaultWeapon);
        }
        Debug.Log("New engineData made!");
    }

    public GameObject MakeObject(Vector3 pos, Quaternion rot, Transform parent)
    {   
        GameObject obj = GameObject.Instantiate(baseStats.enginePrefab, pos, rot, parent);
        Transform weaponPoint = obj.transform.GetChild(0);
        if (weapon != null)
            weapon.MakeObject(weaponPoint.position, weaponPoint.rotation, weaponPoint, true);
        return obj;
        
    }
    
}