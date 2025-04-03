using System;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class StatsManager : MonoBehaviour
{
    //Runtime stats
    private Stats stats;
    //Cached stats 
    private Stats cachedStats;
    private List<Upgrade> appliedUpgrades = new List<Upgrade>();
    private List<StatusEffect> appliedStatusEffects = new List<StatusEffect>();
    private bool isInitialized;

    public virtual void Initialize(Stats s)
    {
        stats = Instantiate(s);
    }

    void Update()
    {
        for (int i=0;i<appliedStatusEffects.Count;i++)   
        {
            if (appliedStatusEffects[i].IsTick)
            {
                cachedStats.basicStats[appliedStatusEffects[i].Stat] += appliedStatusEffects[i].Value*Time.deltaTime;
            }
            if (Time.time >= (appliedStatusEffects[i].InstanceTime + appliedStatusEffects[i].TimeToLive))
            {
                appliedStatusEffects.RemoveAt(i);
                RecacheStats();
            }
            
        }
    }

    public float? GetBasicStat(Stat stat)
    {
        if (cachedStats ==null || cachedStats.basicStats == null) RecacheStats();
        float? statVal = cachedStats.GetBasicStat(stat);
        return statVal;
    }
    
    public void SetBasicStat(Stat stat, float value)
    {
        stats.SetBasicStat(stat, value);
    }

    public Stats GetStats()
    {
        RecacheStats();
        return cachedStats;
    }

    void RecacheStats()
    {
        cachedStats = stats;
        Dictionary<Stat, float> AdditiveModifiers = new Dictionary<Stat, float>();
        Dictionary<Stat, float> MultiplicativeModifiers = new Dictionary<Stat, float>();
        foreach (var upgrade in appliedUpgrades)
        {
            if (upgrade.IsMultiplicative) MultiplicativeModifiers.Add(upgrade.Stat, upgrade.Value);
            else AdditiveModifiers.Add(upgrade.Stat, upgrade.Value);
        }
        foreach (var statusEffect in appliedStatusEffects)
        {
            if (statusEffect.IsMultiplicative) cachedStats.basicStats[statusEffect.Stat] *= statusEffect.Value;
            else cachedStats.ModifyBasicStat(statusEffect.Stat, statusEffect.Value);
        }
    }
}

