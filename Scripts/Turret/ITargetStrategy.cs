using System;
using System.Collections.Generic;
using UnityEngine;

public interface ITargetStrategy
{
    void Target(Stats stats, TurretSO turretSO, bool isEngineControlled, LayerMask targetable, out List<Transform> targets);
}

public static class TurretTargetStrategyFactory
{
    public static ITargetStrategy GetTargetStrategy(Turret turret)
    {
        switch (turret.SubType)
        {
            // Bullet types use the same targeting strategy here.
            case TurretSubType.Single:
            case TurretSubType.Auto:
            case TurretSubType.Burst:
                return new BulletTargetStrategy();
            // Launcher types (or others) could be implemented later.
            default:
                throw new ArgumentException("Invalid turret subtype for Attack strategy, or not implemented yet");
        }
    }
}

public class BulletTargetStrategy : ITargetStrategy
{
    public void Target(Stats stats, TurretSO turretSO, bool isEngineControlled, LayerMask targetable, out List<Transform> targets)
    {
        float range = stats.GetBasicStat(Stat.AttackRange) ?? 0f;
        Vector3 targetPosition = new Vector3();
        targets = null;

        if (isEngineControlled)
        {
            // For player-controlled, use the mouse position.
            targetPosition = GetMouseWorldPosition(turretSO.ModelPrefabData.baseTransform);
        }
        else
        {
            // For automatic targeting, get the closest enemy within range.
            Transform autoTarget = GetTarget(turretSO.ModelPrefabData.turretTransform, range, targetable);
            if (autoTarget != null)
            {
                targetPosition = autoTarget.position;
            }
        }

        if (targetPosition != Vector3.zero)
        {
            Aim(targetPosition, turretSO);
        } 
    }

    /// <summary>
    /// Computes the mouse world position projected onto a horizontal plane at the turret's base height.
    /// </summary>
    private Vector3 GetMouseWorldPosition(Transform baseTransform)
    {
        Camera cam = Camera.main;
        float planeY = baseTransform.position.y;
        Plane horizontalPlane = new Plane(Vector3.up, new Vector3(0, planeY, 0));
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (horizontalPlane.Raycast(ray, out float distanceToPlane))
        {
            return ray.GetPoint(distanceToPlane);
        }
        return baseTransform.position;
    }

    /// <summary>
    /// Adjusts the turret's base and pivot rotations so that it aims at the given target position.
    /// </summary>
    private void Aim(Vector3 targetPosition, TurretSO turretSO)
    {
        Transform baseT = turretSO.ModelPrefabData.baseTransform;
        Transform pivotT = turretSO.ModelPrefabData.pivotTransform;
        Transform firePointT = turretSO.ModelPrefabData.firePointTransform;

        // 1. Rotate the base turret (yaw) to face the target horizontally.
        Vector3 targetPosOnPlane = targetPosition;
        targetPosOnPlane.y = baseT.position.y;  // Align on the same horizontal plane.
        Vector3 directionToTarget = targetPosOnPlane - baseT.position;
        
        if (directionToTarget.sqrMagnitude < 0.0001f)
            return;

        Quaternion desiredBaseRotation = Quaternion.LookRotation(directionToTarget, Vector3.up);
        Vector3 currentEuler = baseT.rotation.eulerAngles;
        baseT.rotation = Quaternion.Euler(currentEuler.x, desiredBaseRotation.eulerAngles.y, currentEuler.z);

        // 2. Rotate the pivot (or turret gun) to aim directly at the target.
        Vector3 directionFromFirePoint = targetPosition - firePointT.position;
        if (directionFromFirePoint.sqrMagnitude < 0.0001f)
            return;

        // Calculate the direction from the child object to the third object
        Vector3 direction2 = targetPosition - firePointT.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction2);
        Quaternion currentRelativeRotation = Quaternion.Inverse(pivotT.rotation) * firePointT.rotation;
        Quaternion topRotation = targetRotation * Quaternion.Inverse(currentRelativeRotation);
        pivotT.rotation = topRotation; 
    }

    /// <summary>
    /// Finds the closest enemy (by tag) within the specified range using a physics overlap sphere.
    /// </summary>
    private Transform GetTarget(Transform turretTransform, float range, LayerMask enemyLayer)
    {
        Collider[] hits = Physics.OverlapSphere(turretTransform.position, range, enemyLayer);
        Transform closestEnemy = null;
        float closestDistanceSqr = Mathf.Infinity;

        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                float distanceSqr = (hit.transform.position - turretTransform.position).sqrMagnitude;
                if (distanceSqr < closestDistanceSqr)
                {
                    closestDistanceSqr = distanceSqr;
                    closestEnemy = hit.transform;
                }
            }
        }
        return closestEnemy;
    }
}


