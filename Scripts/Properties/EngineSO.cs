using UnityEngine;

[CreateAssetMenu(fileName = "New Engine", menuName = "Mitch Kassidy/Engine")]
public class EngineSO : ScriptableObject
{
    public string engineName;
    [TextArea] public string description;

    public float baseHealth = 100f;
    public float baseStorage = 50f;
    public float moveSpeed = 5f;

    public Sprite engineSprite;
    public GameObject enginePrefab;
    public WeaponSO defaultWeapon;

    // Optional: Perks or modifiers
    public float regenRate = 0f;
    public bool immuneToStun = false;
}