namespace DragonSpark.Model.Results;

public sealed class Switch : Variable<bool>, ISwitch
{
	public Switch(bool instance = default) : base(instance) {}
}