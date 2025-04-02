using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class MainWeaponData : ScriptableObject
{
    public enum WeaponType {Launcher, Laser, Shield}
    public enum LauncherType {Single, Auto, Burst}

    public WeaponType weaponType;
    [SerializeReference] public Weapon data = new Weapon();
    

    [System.Serializable]
    public class Weapon
    {
        [SerializeField] public string name;
        [SerializeField] public string description;
    }


    [System.Serializable]
    public class Launcher : Weapon 
    {
        [SerializeField] public float bulletForce;
        [SerializeField] public float bulletDelay;
        [SerializeField] public float bulletDamage;
        [SerializeField] public float explosionRadius;
        [SerializeField] public LauncherType type;
        [SerializeField] public GameObject BulletPrefab;
        public Launcher(){}

        public GameObject Shoot(Transform firePoint)
        {
            GameObject bullet = Instantiate(this.BulletPrefab, firePoint.position, firePoint.rotation);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            rb.AddForce(firePoint.forward * bulletForce, ForceMode.Impulse);
            bullet.GetComponent<BulletHandler>().damage = bulletDamage;
            bullet.GetComponent<BulletHandler>().explosionRadius = explosionRadius;
            return bullet;
        }
    }


    public class Laser : Weapon
    {
        public Laser(){}
    }


    public class Shield : Weapon
    {
        public Shield(){}
    }


    void OnValidate()
    {
        switch(weaponType)
        {
            case WeaponType.Laser:
                if (data != null && data.GetType() != typeof(Laser))
                {
                    data = new Laser();
                }
                break;
            case WeaponType.Shield:
                if (data != null && data.GetType() != typeof(Shield))
                {
                    data = new Shield();
                }
                break;
            case WeaponType.Launcher:
            default:
                if (data != null && data.GetType() != typeof(Launcher))
                {
                    data = new Launcher();
                }
                break;
        }
    }
}
