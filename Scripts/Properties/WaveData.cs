using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "WaveData", menuName = "Scriptable Objects/WaveData")]
public class WaveData : ScriptableObject
{
    [System.Serializable]
    public class WaveEnemy
    {
        public GameObject model;
        public int count;

        // Unity serialization doesn't call constructors when deserializing, so you don't need this.
        // But you can keep it if you ever initialize via script.
        public WaveEnemy(GameObject m, int c)
        {
            model = m;
            count = c;
        }
    }

    [Tooltip("This is the wave number used to sort waves in order.")]
    [SerializeField] private int waveNumber;

    [Tooltip("Enemies per second or similar rate for this wave.")]
    [SerializeField] private int waveRate;

    [Tooltip("List of enemy types and their counts in this wave.")]
    [SerializeField] private List<WaveEnemy> enemies = new List<WaveEnemy>();

    // 👉 Add public accessors for read-only access if needed
    public int WaveNumber => waveNumber;
    public int WaveRate => waveRate;
    public List<WaveEnemy> Enemies => enemies;
}