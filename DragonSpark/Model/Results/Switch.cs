namespace DragonSpark.Model.Results;

public class Switch : Variable<bool>, ISwitch
{
	public static implicit operator bool(Switch instance) => instance.Get();
	public static implicit operator Switch(bool instance) => new(instance);

	public Switch(bool instance = false) : base(instance) {}
}