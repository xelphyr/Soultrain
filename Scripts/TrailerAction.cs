using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

[DisallowMultipleComponent]
public class TrailerAction : MonoBehaviour
{
    public bool isEngineControlled;

    [SerializeField] 
    private TrailerTurretSelector TurretSelector;
    private void Update()
    {
        if (isEngineControlled && Mouse.current.leftButton.isPressed && TurretSelector.ActiveTuret != null)
        {
            TurretSelector.ActiveTuret.Shoot();
        }
    }
}