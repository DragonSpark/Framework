namespace DragonSpark.Composition.Compose;

sealed class LinkedRegistrations : IRegistrations
{
	readonly IRegistrations _first, _second;

	public LinkedRegistrations(IRegistrations first, IRegistrations second)
	{
		_first  = first;
		_second = second;
	}

	public IRegistration Get(IRelatedTypes parameter) => _first.Get(parameter).Then(_second.Get(parameter));
}