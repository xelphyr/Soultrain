using System;
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

    private void Start()
    {
        TurretSelector = GetComponent<TrailerTurretSelector>();
        statsManager = GetComponent<StatsManager>();
    }

    private void Update()
    {
        if (isEngineControlled)
        {
            if (Mouse.current.leftButton.isPressed && TurretSelector.ActiveTurret != null)
            {
                TurretSelector.ActiveTurret.Shoot(statsManager.GetStats());
            }
        }
        else
        {
            
        }
    }
}