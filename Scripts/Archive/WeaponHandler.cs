using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(TurretAimer))]
public class WeaponHandler : MonoBehaviour
{
    public WeaponData weapon;
    public Transform firePoint;

    [Header("Targeting")]
    public LayerMask enemyLayer;

    public float targetingRange;
    public TurretAimer aim;
    
    [Header("Ownership")]
    public bool isEngineControlled;
    
    private float cooldown;
    private Transform currentTarget;
    static readonly Collider[] hitsBuffer = new Collider[50]; 

    void Start()
    {
        aim = GetComponent<TurretAimer>();
    }

    void FixedUpdate()
    {
        if (weapon == null || weapon.baseStats.weaponType == (WeaponType)MainWeaponData.WeaponType.Launcher)
        {
            Debug.Log(
                $"Its not valid or sth, wepon null is {weapon == null} and wepon type is launcher is {weapon.baseStats.weaponType == (WeaponType)MainWeaponData.WeaponType.Launcher}");
        }

        if (isEngineControlled)
        {
            aim.PlayerLookAt();
            if (weapon.CooldownTick(Time.fixedDeltaTime) && Mouse.current.leftButton.isPressed)
            {
                weapon.Shoot(firePoint);
            }
        }
        else
        {
            GetClosestEnemy();
            Debug.Log($"Thinking of targeting {currentTarget.gameObject.name} at {currentTarget.position}, will if {Vector3.Distance(currentTarget.position, transform.position) < weapon.baseStats.weaponRange}");
            if (currentTarget != null && Vector3.Distance(currentTarget.position, transform.position) <
                weapon.baseStats.weaponRange)
            {
                aim.AutoLookAt(currentTarget.position);
                if (weapon.CooldownTick(Time.fixedDeltaTime))
                {
                    weapon.Shoot(firePoint);
                }
            }

        } 
        
    }
    

    void GetClosestEnemy()
    {
        int numHits = Physics.OverlapSphereNonAlloc(transform.position, targetingRange, hitsBuffer, enemyLayer);
        float closestSqrDistance = float.MaxValue;
        Transform closest = null;
        Vector3 selfPos = transform.position;
        for (int i = 0; i<numHits; i++)
        {
            Collider c = hitsBuffer[i];
            float sqrDist = (selfPos- c.transform.position).sqrMagnitude;
            if (sqrDist < closestSqrDistance)
            {
                closestSqrDistance = sqrDist;
                closest = c.transform;
                
            }
        }
        currentTarget = closest;
    }
}
