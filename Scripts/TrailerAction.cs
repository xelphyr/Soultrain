using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

[DisallowMultipleComponent]
[RequireComponent(typeof(TrailerTurretSelector)), RequireComponent(typeof(StatsManager))]
public class TrailerAction : MonoBehaviour
{
    public bool isEngineControlled;

    [SerializeField] 
    private TrailerTurretSelector TurretSelector;

    public StatsManager statsManager;

    private List<Transform> targets;

    private void Start()
    {
        TurretSelector = GetComponent<TrailerTurretSelector>();
        statsManager = GetComponent<StatsManager>();
    }

    private void Update()
    {
        TurretSelector.ActiveTurret.Target(statsManager.GetStats(), isEngineControlled, out targets);
        TurretSelector.ActiveTurret.TryToAttack(statsManager.GetStats(), isEngineControlled);
    }
}