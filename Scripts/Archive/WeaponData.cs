using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class WeaponData
{
    public WeaponSO baseStats;
    public float cooldownTimer;
    private float cooldown;
    private Queue<GameObject> pooledBullets = new Queue<GameObject>();
    private int poolSize;

    public WeaponData(WeaponSO so)
    {
        Debug.Log("New weaponData being made");
        baseStats = so;
        cooldown = baseStats.bulletDelay;
        cooldownTimer = cooldown;
    }

    public GameObject MakeObject(Vector3 pos, Quaternion rot, Transform parent, bool isEngineControlled)
    {
        GameObject weapon = GameObject.Instantiate(baseStats.turretPrefab, pos, rot, parent);
        WeaponHandler wh = weapon.GetComponent<WeaponHandler>();
        wh.weapon = this;
        wh.isEngineControlled = isEngineControlled;
        wh.targetingRange = baseStats.weaponRange;
        GeneratePool();
        return weapon;
    }

    void GeneratePool()
    {
        poolSize = (int) Mathf.Round(5f / cooldown);
        for (int i = 0; i < poolSize; i++)
        {
            GameObject bullet = GameObject.Instantiate(baseStats.bulletPrefab);
            AddToPool(bullet);
        }
        Debug.Log(pooledBullets.Count);
    }

    GameObject Depool(Transform t)
    {
        if (poolSize == 0)
        {
            GameObject temp = GameObject.Instantiate(baseStats.bulletPrefab);
            temp.SetActive(false);
            pooledBullets.Enqueue(temp);
        }
        GameObject bullet = pooledBullets.Dequeue();
        
        bullet.transform.SetPositionAndRotation(t.position, t.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        bullet.SetActive(true);
        return bullet;
    }
    
    public void AddToPool(GameObject bullet)
    {
        bullet.SetActive(false);
        pooledBullets.Enqueue(bullet);
    }
    public bool CooldownTick(float time)
    {
        if (cooldownTimer >= 0f)
        {
            cooldownTimer -= time;
            return false;
        }

        return true;
    }
    
    public GameObject Shoot(Transform firePoint)
    {
        cooldownTimer = cooldown;
        if (baseStats.weaponType != WeaponType.Launcher || baseStats.bulletPrefab == null)
            return null;

        GameObject bullet = Depool(firePoint);

        BulletHandler handler = bullet.GetComponent<BulletHandler>();
        if (handler)
        {
            handler.damage = baseStats.bulletDamage;
            //handler.explosionRadius = baseStats.explosionRadius;
            handler.pool = this;
            handler.trail.Clear();
            //handler.rb.AddForce(firePoint.forward * baseStats.bulletForce, ForceMode.Impulse);
        }

        return bullet;
    }
    
}