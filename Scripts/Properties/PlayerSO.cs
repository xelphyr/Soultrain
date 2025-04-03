using UnityEngine;

[CreateAssetMenu(fileName = "Player", menuName = "Player")]
public class PlayerSO : ScriptableObject
{
    public string engineName;
    [TextArea] public string description;

    public Stats stats;
    
}