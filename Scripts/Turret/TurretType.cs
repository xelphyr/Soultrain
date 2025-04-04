using System.Collections.Generic;
using System;
using System.Linq;

public enum TurretType
{
    Bullet,
    Launcher,
    Laser,
    Shield
}

public enum TurretSubType
{
    // Bullet subtypes
    Single,
    Auto,
    Burst,
    Multi,
    Shotgun,
    // Launcher subtypes
    Homing,
    Missile,
    Cannon,
    // Laser subtypes
    Arc,
    Charge,
    // Shield subtypes
    Force,
    Status
}

public static class TurretTypeValidator
{
    private static readonly Dictionary<TurretType, TurretSubType[]> validSubTypes = new Dictionary<TurretType, TurretSubType[]>
    {
        { TurretType.Bullet, new[] { TurretSubType.Single, TurretSubType.Auto, TurretSubType.Burst, TurretSubType.Multi, TurretSubType.Shotgun} },
        { TurretType.Launcher, new[] { TurretSubType.Homing, TurretSubType.Missile, TurretSubType.Cannon } },
        { TurretType.Laser, new[] { TurretSubType.Arc, TurretSubType.Charge } },
        { TurretType.Shield, new[] { TurretSubType.Force, TurretSubType.Status } },
    };

    public static bool IsValidSubType(TurretType type, TurretSubType subType)
    {
        return validSubTypes.TryGetValue(type, out var subTypes) && subTypes.Contains(subType);
    }
}
public class Turret
{
    public TurretType Type { get; set; }
    private TurretSubType _subType;
    
    public TurretSubType SubType
    {
        get => _subType;
        set
        {
            if (!TurretTypeValidator.IsValidSubType(Type, value))
                throw new ArgumentException($"Invalid subtype {value} for turret type {Type}");
            _subType = value;
        }
    }
}
