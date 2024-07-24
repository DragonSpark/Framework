namespace DragonSpark.Model.Results;

public sealed class EnabledSwitch : Switch
{
	public Switch Enabled { get; }

	public EnabledSwitch(bool instance = default) : this(new(true), instance) {}

	public EnabledSwitch(Switch enabled, bool instance = default) : base(instance) => Enabled = enabled;
}