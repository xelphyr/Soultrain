using UnityEngine;
using System.Collections.Generic;

public class BulletHandler : MonoBehaviour
{
    [SerializeField] public float damage;
    [SerializeField] public float explosionRadius;
    public float SurvivalTime = 5f;
    float timeToLive;
    public WeaponData pool;
    public Rigidbody rb;
    public TrailRenderer trail;
    
    void OnEnable()
    {
        timeToLive = SurvivalTime;
    }

    // Update is called once per frame
    void Update()
    {
        timeToLive -= Time.deltaTime;
        if(timeToLive<=0f)
        {
            pool.AddToPool(gameObject);
        }
    }

    private void OnCollisionEnter(Collision other) {
        Debug.Log("We hit sth lol: ");
        if (explosionRadius > 0f)
        {
            Explode();
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Enemy") || other.gameObject.layer == LayerMask.NameToLayer("Enemy") )
        {
            Damage(other.gameObject);
        }
        pool.AddToPool(gameObject);
        return;
    }
    
    void Explode()
    {

    }
    void Damage(GameObject other)
    {
        Health h = other.GetComponent<Health>();

        if (h != null)
        {
            h.TakeDamage(damage);
        }     
    }
}
