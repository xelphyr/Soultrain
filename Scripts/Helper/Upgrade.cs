public readonly struct Upgrade
{
    public string Name { get; }
    public string Tag{ get; }
    public Stat Stat{ get; }
    public float Value{ get; }
    public bool IsMultiplicative{ get; }

    public Upgrade(string name, string tag, Stat stat, float value, bool isMultiplicative)
    {
        this.Name = name;
        this.Tag = tag;
        this.Stat = stat;
        this.Value = value;
        this.IsMultiplicative = isMultiplicative;
    }
}
