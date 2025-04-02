using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "WaveDatabase", menuName = "Scriptable Objects/WaveDatabase")]
public class WaveDatabase : ScriptableObject
{
    [SerializeField] private List<WaveData> allWaves = new List<WaveData>();

    public List<WaveData> AllWaves => allWaves;

#if UNITY_EDITOR
    // Method to refresh the wave list in editor
    public void RefreshWaveList()
    {
        allWaves.Clear();

        string[] guids = UnityEditor.AssetDatabase.FindAssets("t:WaveData", new[] { "Assets/Waves" });

        foreach (var guid in guids)
        {
            string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
            WaveData wave = UnityEditor.AssetDatabase.LoadAssetAtPath<WaveData>(path);
            if (wave != null)
                allWaves.Add(wave);
        }

        // Optional: Sort by wave number
        allWaves.Sort((a, b) => a.WaveNumber.CompareTo(b.WaveNumber));

        UnityEditor.EditorUtility.SetDirty(this);
        UnityEditor.AssetDatabase.SaveAssets();

        Debug.Log($"[WaveDatabase] Refreshed: {allWaves.Count} waves loaded.");
    }
#endif
}