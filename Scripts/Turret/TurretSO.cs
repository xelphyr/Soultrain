using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

[CreateAssetMenu(fileName = "Turret", menuName = "Turrets/Turret", order = 0)]
public class TurretSO : ScriptableObject
{
    public Turret Type;
    public string Name;
    public GameObject ModelPrefab;
    public Vector3 SpawnPoint;
    public Vector3 SpawnRotation;

    public ShootConfigSO ShootConfig;
    public TrailConfigSO TrailConfig;
    public Stats baseStats;

    private MonoBehaviour ActiveMonoBehaviour;
    private GameObject Model;
    private float LastShootTime;
    private ParticleSystem ShootSystem;
    private ObjectPool<TrailRenderer> TrailPool;

    public void Spawn(Transform Parent, MonoBehaviour ActiveMonoBehaviour)
    {
        this.ActiveMonoBehaviour = ActiveMonoBehaviour;
        LastShootTime = 0;
        TrailPool = new ObjectPool<TrailRenderer>(CreateTrail);
        Model = Instantiate(ModelPrefab);
        Model.transform.SetParent(Parent, false);
        Model.transform.localPosition = SpawnPoint;
        Model.transform.localRotation = Quaternion.Euler(SpawnRotation);
        
        ShootSystem = Model.GetComponentInChildren<ParticleSystem>();
        
    }

    public void Shoot(Stats stats)
    {
        ShootConfig.Shoot(stats);
    }
    

    private TrailRenderer CreateTrail()
    {
        GameObject instance = new GameObject("BulletTrail");
        TrailRenderer trail = instance.AddComponent<TrailRenderer>();
        trail.colorGradient = TrailConfig.Color;
        trail.material = TrailConfig.Material;
        trail.widthCurve = TrailConfig.WidthCurve;
        trail.time = TrailConfig.Duration;
        trail.minVertexDistance = TrailConfig.MinVertexDistance;

        trail.emitting = false;
        trail.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

        return trail;
    }

    private IEnumerator PlayTrail(Transform StartPointTransform, Vector3 EndPoint, RaycastHit Hit, float Damage)
    {
        TrailRenderer instance = TrailPool.Get();
        instance.gameObject.SetActive(true);
        instance.transform.position = StartPointTransform.position;
        yield return null;

        instance.emitting = true;
        Vector3 StartPoint = StartPointTransform.position;
        float distance = Vector3.Distance(StartPoint, EndPoint);
        float remainingDistance = distance;
        while (remainingDistance > 0)
        {
            instance.transform.position = Vector3.Lerp(
                StartPoint,
                EndPoint,
                Mathf.Clamp01(1 - (remainingDistance / distance))
            );
            remainingDistance -= TrailConfig.SimulationSpeed*Time.deltaTime;
            yield return null;
        }

        instance.transform.position = EndPoint;

        if (Hit.collider != null)
        {
            if (Hit.collider.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(Damage);
            }
        }
        
        yield return new WaitForSeconds(TrailConfig.Duration);
        yield return null;
        instance.emitting = false;
        instance.gameObject.SetActive(false);
        TrailPool.Release(instance);
    }


}