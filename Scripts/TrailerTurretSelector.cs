using System;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class TrailerTurretSelector : MonoBehaviour
{
    [SerializeField] 
    private Transform TurretParent;
    

    [Space] 
    [Header("RunTimeFilled")] 
    public TurretSO ActiveTurret;

    private void Start()
    {
        ActiveTurret.Spawn(TurretParent, this);
    }
    
}