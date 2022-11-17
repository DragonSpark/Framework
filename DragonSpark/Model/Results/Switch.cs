namespace DragonSpark.Model.Results;

public class Switch : Variable<bool>, ISwitch
{
	public Switch(bool instance = default) : base(instance) {}
}