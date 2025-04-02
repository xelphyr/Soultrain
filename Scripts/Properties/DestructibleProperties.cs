using UnityEngine;

[CreateAssetMenu(fileName = "DestructibleProperties", menuName = "Scriptable Objects/DestructibleProperties")]
public class DestructibleProperties : ScriptableObject
{
    [SerializeField] public float Name;
    [SerializeField] public float Health;

}
