namespace DragonSpark.Model.Results;

public sealed class AssignedAwareVariable<T> : MutationAware<T?>
{
	public AssignedAwareVariable() : this(new Variable<T>()) {}

	public AssignedAwareVariable(IMutable<T?> mutable) : base(mutable, new IsAssigned<T>(mutable)) {}
}

// TODO:
/*public class SwitchAwareVariable<T> : MutationAware<T?>
{
	public SwitchAwareVariable(IMutable<bool> @switch, IMutable<T?> mutable)
		: base(mutable, A.Condition(new IsSwitched(@switch)).Then().And(new VisitedAwareVariable<T>())) {}
}*/

/*sealed class IsSwitched : Result<bool>, ICondition
{
	public IsSwitched(ICondition previous, IResult<bool> store) : base(store) {}

	public bool Get(None parameter) => Get();
}*/