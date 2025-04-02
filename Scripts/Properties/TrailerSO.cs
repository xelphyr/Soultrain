using UnityEngine;

[CreateAssetMenu(fileName = "New Trailer", menuName = "Mitch Kassidy/Trailer")]
public class TrailerSO : ScriptableObject
{
    public string trailerName;
    [TextArea] public string description;

    public float baseHealth = 50f;
    public float baseStorage = 25f;

    public GameObject trailerPrefab;
    public WeaponSO defaultWeapon;  // Can be null if no starting weapon

    public bool supportsMultipleWeapons = false;

    // Optional visuals
    public Sprite trailerSprite;
}