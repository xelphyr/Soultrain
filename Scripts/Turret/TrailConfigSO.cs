using UnityEngine;

[CreateAssetMenu(fileName = "Trail Config", menuName = "Turrets/Turret Trail", order = 4)]
public class TrailConfigSO : ScriptableObject
{
    public Material Material;
    public AnimationCurve WidthCurve;
    public float Duration = 0.5f;
    public float MinVertexDistance = 0.1f;
    public Gradient Color;

    public float MissDistance = 100f;
    public float SimulationSpeed = 100f;
}