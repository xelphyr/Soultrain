using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Turret", menuName = "Turrets/Turret", order = 0)]
public class TurretSO : ScriptableObject
{
    public Turret Type;
    public string Name;
    public GameObject ModelPrefab;
    [HideInInspector] public TurretPrefabData ModelPrefabData;
    public Vector3 SpawnPoint;
    public Vector3 SpawnRotation;

    public ITargetStrategy TargetStrategy;
    public IAttackStrategy AttackStrategy;
    public TrailConfigSO TrailConfig;
    public Stats baseStats;

    public LayerMask HitMask;

    public MonoBehaviour ActiveMonoBehaviour;
    private GameObject Model;
    [HideInInspector] public ParticleSystem ShootSystem;
    private ObjectPool<TrailRenderer> TrailPool;

    public void Spawn(Transform Parent, MonoBehaviour ActiveMonoBehaviour)
    {
        this.ActiveMonoBehaviour = ActiveMonoBehaviour;
        TrailPool = new ObjectPool<TrailRenderer>(CreateTrail);
        Model = Instantiate(ModelPrefab, SpawnPoint, Quaternion.Euler(SpawnRotation), Parent);
        
        ModelPrefabData = Model.GetComponent<TurretPrefabData>();
        ShootSystem = ModelPrefabData.firePoint;
        AttackStrategy = TurretAttackStrategyFactory.GetAttackStrategy(Type);
        TargetStrategy = TurretTargetStrategyFactory.GetTargetStrategy(Type);
    }

    public void TryToAttack(Stats stats, bool isEngineControlled)
    {
        AttackStrategy.TryToAttack(stats, this, isEngineControlled);
    }

    public void Target(Stats stats, bool isEngineControlled, out List<Transform> targets)
    {
        TargetStrategy.Target(stats, this, isEngineControlled, HitMask, out targets);
    }
    

    public TrailRenderer CreateTrail()
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

    public IEnumerator PlayTrail(Transform StartPointTransform, Vector3 EndPoint, RaycastHit Hit, float Damage)
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