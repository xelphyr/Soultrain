using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
public class WaveHandler : MonoBehaviour
{
    public List<WaveData> waves = new List<WaveData>();
    public List<Transform> spawnpoints = new List<Transform>();
    public TextMeshProUGUI waveText;
    WaveData currentWave;

    int waveNumber = -1;
    List<GameObject> waveEnemies = new List<GameObject>();
    List<GameObject> buffer = new List<GameObject>();
    

    bool isSpawning = false;
    float spawnCooldown = 0f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Next Wave Start
        if (waveEnemies.Count == 0 && waveNumber < (waves.Count-1))
        {
            waveNumber += 1;
            isSpawning = true;
            currentWave = waves[waveNumber];
            AddToBuffer(currentWave.Enemies);
            if (waveText != null)
                waveText.text = "Wave: " + (waveNumber + 1);
        }

        //Spawning
        if (isSpawning)
        {
            Spawn();
        }

        //Enemy Death Check
        for (int i = waveEnemies.Count - 1; i >= 0; i--)
        {
            if (waveEnemies[i] == null) waveEnemies.RemoveAt(i);
        }

            
    }

    void AddToBuffer(List<WaveData.WaveEnemy> enemies)
    {
        foreach (var enemy in enemies)
        {
            buffer.AddRange(Enumerable.Repeat(enemy.model, enemy.count));
        }
    }

    void Spawn()
    {
        if (spawnCooldown <= 0f)
        {
            spawnCooldown = currentWave.WaveRate;
            int randomIndex = Random.Range(0, buffer.Count);
            Vector3 randomPosition = spawnpoints[Random.Range(0, spawnpoints.Count)].position;
            waveEnemies.Add(Instantiate(buffer[randomIndex], randomPosition, Quaternion.identity));
            buffer.RemoveAt(randomIndex);
            if (buffer.Count == 0) isSpawning = false;
        }
        spawnCooldown -= Time.deltaTime;
    }
}
