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
    // Launcher subtypes
    Homing,
    Missile,
    Cannon,
    // Laser subtypes
    Continuous,
    Pulsed,
    // Shield subtypes
    Energy,
    Kinetic
}

public static class TurretTypeValidator
{
    private static readonly Dictionary<TurretType, TurretSubType[]> validSubTypes = new Dictionary<TurretType, TurretSubType[]>
    {
        { TurretType.Bullet, new[] { TurretSubType.Single, TurretSubType.Auto, TurretSubType.Burst, TurretSubType.Multi } },
        { TurretType.Launcher, new[] { TurretSubType.Homing, TurretSubType.Missile, TurretSubType.Cannon } },
        { TurretType.Laser, new[] { TurretSubType.Continuous, TurretSubType.Pulsed } },
        { TurretType.Shield, new[] { TurretSubType.Energy, TurretSubType.Kinetic } },
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
