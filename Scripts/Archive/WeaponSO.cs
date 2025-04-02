using UnityEngine;

[System.Serializable]
public enum WeaponType { Bullet, Launcher, Laser, Shield }
[System.Serializable]
public enum BulletType {Single, Auto, Burst, Multi}
[System.Serializable]
public enum LauncherType {Homing, Cannon}
[System.Serializable]
public enum LaserType {Arc, Charge}
[System.Serializable]
public enum ShieldType {Shield, StatusPos, StatusNeg}

[CreateAssetMenu(fileName = "New Weapon", menuName = "Mitch Kassidy/Weapon")]
public class WeaponSO : ScriptableObject
{
    public string weaponName;
    [TextArea] public string description;

    public WeaponType weaponType;
    public float weaponRange;
    
    public GameObject ammoPrefab;
    
    public class BulletSettings
    {
        public BulletType bulletType;
        public float bulletForce;
        public float burstCooldown;
        public float cooldown;
        public float bulletDamage;
        public float piercingDamageReduction;
        public BulletSettings() { }
    }
    
    [SerializeReference]
    public BulletSettings bulletSettings = new BulletSettings();

    [Header("Launcher Settings")]
    public LauncherType launcherType;
    public GameObject bulletPrefab;
    public float bulletDelay = 0.5f;
    public float bulletDamage = 10f;
    public float piercingDamageReduction = 50f;

    // For lasers, shields, etc. you can expand below:
    [Header("Laser Settings")]
    public float laserDamagePerSecond = 5f;
    public float laserLength = 20f;

    [Header("Shield Settings")]
    public float shieldDuration = 5f;
    public float shieldCooldown = 10f;

    // Add other shared visuals
    public Sprite icon;
    public GameObject turretPrefab;
}