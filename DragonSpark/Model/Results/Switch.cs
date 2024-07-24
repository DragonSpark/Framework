namespace DragonSpark.Model.Results;

public class Switch : Variable<bool>, ISwitch
{
	public static implicit operator bool(Switch instance) => instance.Get();

	public Switch(bool instance = default) : base(instance) {}
}