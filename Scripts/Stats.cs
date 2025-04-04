using System;
using UnityEngine;
using System.Collections.Generic;
using Alchemy.Serialization;

[CreateAssetMenu(menuName = "Stats/Empty")]
[AlchemySerialize]
public partial class Stats : ScriptableObject
{
    [AlchemySerializeField, NonSerialized]
    public Dictionary<Stat, float> basicStats = new Dictionary<Stat, float>();

    protected void AddStat(Stat stat, float value)
    {
        if (!basicStats.ContainsKey(stat)) basicStats.Add(stat, value);
    }
    public float? GetBasicStat(Stat stat)
    {
        if (basicStats.TryGetValue(stat, out float value))
        {
            return value;
        }
        else
        {
            Debug.LogError($"Tried to get {stat}, but stat {stat} does not exist for {this.name}.");
            return null;
        }
    }
    public void SetBasicStat(Stat stat, float value)
    {
        if (basicStats.TryGetValue(stat, out _))
        {
            basicStats[stat] = value;
        }
        else
        {
            Debug.LogError($"Tried to modify {stat}, but stat {stat} does not exist for {this.name}.");
        }
    }
    
    public void ModifyBasicStat(Stat stat, float value)
    {
        if (basicStats.TryGetValue(stat, out _))
        {
            basicStats[stat] += value;
        }
        else
        {
            Debug.LogError($"Tried to modify {stat}, but stat {stat} does not exist for {this.name}.");
        }
    }
}

[CreateAssetMenu(menuName = "Stats/Trailer")]
[AlchemySerialize]
public partial class TrailerStats : Stats
{
    void OnEnable()
    {
        AddStat(Stat.Health, 0f);
    }
}

[CreateAssetMenu(menuName = "Stats/Player")]
[AlchemySerialize]
public partial class PlayerStats : Stats
{
    void OnEnable()
    {
        AddStat(Stat.Health, 0f);
        AddStat(Stat.Speed, 0f);
        AddStat(Stat.RotationSpeed, 0f);
    }
}

[CreateAssetMenu(menuName = "Stats/Enemy")]
[AlchemySerialize]
public partial class EnemyStats : Stats
{
    void OnEnable()
    {
        AddStat(Stat.Health, 0f);
        AddStat(Stat.Speed, 0f);
        AddStat(Stat.InstantDamage, 0f);
    }
}

[CreateAssetMenu(menuName = "Stats/Turret")]
[AlchemySerialize]
public partial class TurretStats : Stats
{
    void OnEnable()
    {
        AddStat(Stat.Delay, 0f);
    }
}