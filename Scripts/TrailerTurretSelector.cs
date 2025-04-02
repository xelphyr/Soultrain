using System;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class TrailerTurretSelector : MonoBehaviour
{
    [SerializeField] 
    private TurretType Turret;

    [SerializeField] 
    private Transform TurretParent;

    [SerializeField] 
    private List<TurretSO> Turrets;

    [Space] 
    [Header("RunTimeFilled")] 
    public TurretSO ActiveTuret;

    private void Start()
    {  
        TurretSO turret = Turrets.Find(turret => turret.Type == Turret);
        if (turret == null)
        {
            Debug.LogError($"No TurretSO found for TurretType: {Turret}");
            return;
        }
        ActiveTuret = turret;
        turret.Spawn(TurretParent, this);
    }
    
}