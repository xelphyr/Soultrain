/*using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WeaponSO))]
public class WeaponSOEditor : Editor
{
    SerializedProperty weaponName;
    SerializedProperty description;
    SerializedProperty weaponType;
    SerializedProperty weaponRange;
    SerializedProperty bulletSettings;

    SerializedProperty launcherType;
    SerializedProperty bulletPrefab;
    SerializedProperty bulletForce;
    SerializedProperty bulletDelay;
    SerializedProperty bulletDamage;
    SerializedProperty explosionRadius;

    SerializedProperty laserDamagePerSecond;
    SerializedProperty laserLength;

    SerializedProperty shieldDuration;
    SerializedProperty cooldown;

    SerializedProperty icon;
    SerializedProperty turretPrefab;

    void OnEnable()
    {
        weaponName = serializedObject.FindProperty("weaponName");
        description = serializedObject.FindProperty("description");
        weaponType = serializedObject.FindProperty("weaponType");
        weaponRange = serializedObject.FindProperty("weaponRange");
        bulletSettings = serializedObject.FindProperty("bulletSettings");

        launcherType = serializedObject.FindProperty("launcherType");
        bulletPrefab = serializedObject.FindProperty("bulletPrefab");
        bulletForce = serializedObject.FindProperty("bulletForce");
        bulletDelay = serializedObject.FindProperty("bulletDelay");
        bulletDamage = serializedObject.FindProperty("bulletDamage");
        explosionRadius = serializedObject.FindProperty("explosionRadius");

        laserDamagePerSecond = serializedObject.FindProperty("laserDamagePerSecond");
        laserLength = serializedObject.FindProperty("laserLength");

        shieldDuration = serializedObject.FindProperty("shieldDuration");
        cooldown = serializedObject.FindProperty("cooldown");

        icon = serializedObject.FindProperty("icon");
        turretPrefab = serializedObject.FindProperty("turretPrefab");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(weaponName);
        EditorGUILayout.PropertyField(description);
        EditorGUILayout.PropertyField(weaponType);
        EditorGUILayout.PropertyField(weaponRange);
        EditorGUILayout.PropertyField(bulletSettings);

        WeaponType type = (WeaponType)weaponType.enumValueIndex;

        EditorGUILayout.Space(8);
        EditorGUILayout.LabelField("Settings", EditorStyles.boldLabel);

        if (type == WeaponType.Launcher)
        {
            EditorGUILayout.PropertyField(launcherType);
            EditorGUILayout.PropertyField(bulletPrefab);
            EditorGUILayout.PropertyField(bulletForce);
            EditorGUILayout.PropertyField(bulletDelay);
            EditorGUILayout.PropertyField(bulletDamage);
            EditorGUILayout.PropertyField(explosionRadius);
        }
        else if (type == WeaponType.Laser)
        {
            EditorGUILayout.PropertyField(laserDamagePerSecond);
            EditorGUILayout.PropertyField(laserLength);
        }
        else if (type == WeaponType.Shield)
        {
            EditorGUILayout.PropertyField(shieldDuration);
            EditorGUILayout.PropertyField(cooldown);
        }

        EditorGUILayout.Space(8);
        EditorGUILayout.LabelField("Visuals", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(icon);
        EditorGUILayout.PropertyField(turretPrefab);

        serializedObject.ApplyModifiedProperties();
    }
}
*/