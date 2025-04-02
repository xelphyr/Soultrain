#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Linq;

public class WaveDatabaseEditor : AssetPostprocessor
{
    // This runs whenever assets are imported, deleted, or moved
    static void OnPostprocessAllAssets(
        string[] importedAssets,
        string[] deletedAssets,
        string[] movedAssets,
        string[] movedFromAssetPaths)
    {
        bool waveDataChanged = importedAssets.Any(path => path.Contains("/Waves/") && path.EndsWith(".asset")) ||
                               deletedAssets.Any(path => path.Contains("/Waves/") && path.EndsWith(".asset")) ||
                               movedAssets.Any(path => path.Contains("/Waves/") && path.EndsWith(".asset")) ||
                               movedFromAssetPaths.Any(path => path.Contains("/Waves/") && path.EndsWith(".asset"));

        if (waveDataChanged)
        {
            string[] guids = AssetDatabase.FindAssets("t:WaveDatabase");
            if (guids.Length == 0)
            {
                Debug.LogWarning("[WaveDatabase] No WaveDatabase asset found.");
                return;
            }

            string dbPath = AssetDatabase.GUIDToAssetPath(guids[0]);
            WaveDatabase db = AssetDatabase.LoadAssetAtPath<WaveDatabase>(dbPath);

            if (db != null)
            {
                db.RefreshWaveList();
                Debug.Log("[WaveDatabase] Auto-refreshed due to asset change.");
            }
        }
    }
}
#endif
