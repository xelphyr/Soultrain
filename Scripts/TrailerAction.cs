using UnityEngine;
using UnityEngine.PlayerLoop;

[DisallowMultipleComponent]
public class TrailerAction : MonoBehaviour
{
    bool isEngineControlled = false;

    [SerializeField] 
    private TrailerTurretSelector TurretSelector;
    private void Update()
    {
        if (isEngineControlled)
        {
            
        }
    }
        
}