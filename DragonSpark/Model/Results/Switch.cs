namespace DragonSpark.Model.Results;

public class Switch : Variable<bool>, ISwitch
{
	public static implicit operator bool(Switch instance) => instance.Get();

	public Switch(bool instance = default) : base(instance) {}
}

// TODO

public sealed class EnabledSwitch : Switch
{
	public Switch Enabled { get; }

	public EnabledSwitch(bool instance = default) : this(new(true), instance) {}

	public EnabledSwitch(Switch enabled, bool instance = default) : base(instance) => Enabled = enabled;
}