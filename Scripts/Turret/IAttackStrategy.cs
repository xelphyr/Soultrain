using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public interface IAttackStrategy
{
    void TryToAttack(Stats stats, TurretSO turretSO, bool isEngineControlled, List<Transform> targets = null);
}

public static class TurretAttackStrategyFactory
{
    public static IAttackStrategy GetAttackStrategy(Turret turret)
    {
        switch (turret.SubType)
        {
            //Bullet
            case TurretSubType.Single:
                return new SingleBulletAttackStrategy();
            case TurretSubType.Auto:
                return new AutoBulletAttackStrategy();
            case TurretSubType.Burst:
                return new BurstBulletAttackStrategy();
            //Launcher
            default:
                throw new ArgumentException("Invalid turret subtype for Attack strategy, or not implemented yet");
        }
    }
}

public abstract class BulletAttackStrategy
{
    protected void Shoot(TurretSO turretSO, float damage)
    {
        Vector3 shootDirection = turretSO.ShootSystem.transform.forward;
        shootDirection.Normalize();
        if (Physics.Raycast(
                turretSO.ShootSystem.transform.position,
                shootDirection,
                out RaycastHit hit,
                float.MaxValue,
                turretSO.HitMask))
        {
            turretSO.ActiveMonoBehaviour.StartCoroutine(
                turretSO.PlayTrail(
                    turretSO.ShootSystem.transform,
                    hit.point,
                    hit,
                    damage)
            );
        }
        else
        {
            turretSO.ActiveMonoBehaviour.StartCoroutine(
                turretSO.PlayTrail(
                    turretSO.ShootSystem.transform,
                    turretSO.ShootSystem.transform.position + (shootDirection* turretSO.TrailConfig.MissDistance),
                    new RaycastHit(),
                    damage)
            );
        }
    }
}

public class SingleBulletAttackStrategy : BulletAttackStrategy, IAttackStrategy
{
    private float LastShootTime;
    public void TryToAttack(Stats stats, TurretSO turretSO, bool isEngineControlled, List<Transform> targets = null)
    {
        if (Time.time > stats.GetBasicStat(Stat.Delay) + LastShootTime && CanFire(isEngineControlled))
        {
            LastShootTime = Time.time;
            Shoot(turretSO, stats.GetBasicStat(Stat.InstantDamage)??0f);
        }
    }
    
    private bool CanFire(bool isEngineControlled)
    {
        return !isEngineControlled || Mouse.current.leftButton.wasPressedThisFrame;
    }
}

public class AutoBulletAttackStrategy : BulletAttackStrategy, IAttackStrategy
{
    private float LastShootTime;
    public void TryToAttack(Stats stats, TurretSO turretSO, bool isEngineControlled, List<Transform> targets = null)
    {
        if (Time.time > stats.GetBasicStat(Stat.Delay) + LastShootTime && CanFire(isEngineControlled))
        {
            LastShootTime = Time.time;
            Shoot(turretSO, stats.GetBasicStat(Stat.InstantDamage)??0f);
        }
    }
    private bool CanFire(bool isEngineControlled)
    {
        return !isEngineControlled || Mouse.current.leftButton.isPressed;
    }
}

public class BurstBulletAttackStrategy : BulletAttackStrategy, IAttackStrategy
{
    private float LastShootTime;
    private float cooldown;
    private int bulletsFired;
    public void TryToAttack(Stats stats, TurretSO turretSO, bool isEngineControlled, List<Transform> targets = null)
    {
        if (Time.time > cooldown + LastShootTime)
        {
            if (bulletsFired >= (int)(stats.GetBasicStat(Stat.BurstAmount) ?? 1f) && CanFire(isEngineControlled))
            {
                cooldown = stats.GetBasicStat(Stat.BurstDelay) ?? 0f;
                bulletsFired = 0;
            }
            else
            {
                cooldown = stats.GetBasicStat(Stat.Delay) ?? 0f;
                bulletsFired += 1;
            }
            LastShootTime = Time.time;
            Shoot(turretSO, stats.GetBasicStat(Stat.InstantDamage)??0f);
        }
    }
    
    private bool CanFire(bool isEngineControlled)
    {
        return !isEngineControlled || Mouse.current.leftButton.isPressed;
    }
}

