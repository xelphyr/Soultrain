public readonly struct StatusEffect
{
    public string Name { get; }
    public string Tag{ get; }
    public Stat Stat{ get; }
    public float Value{ get; }
    public bool IsTick{ get; }
    public bool IsMultiplicative { get;  }
    public float InstanceTime{ get; }
    public float TimeToLive{ get; }

    public StatusEffect(string name, string tag, Stat stat, float value, bool isTick, bool isMultiplicative, float instanceTime, float timeToLive)
    {
        this.Name = name;
        this.Tag = tag;
        this.Stat = stat;
        this.Value = value;
        this.IsTick = isTick;
        this.IsMultiplicative = isMultiplicative;
        this.InstanceTime = instanceTime;
        this.TimeToLive = timeToLive;
    }
}