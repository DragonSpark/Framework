namespace DragonSpark.Model.Results;

public sealed class AssignedAwareVariable<T> : MutationAware<T?>
{
	public AssignedAwareVariable() : this(new Variable<T>()) {}

	public AssignedAwareVariable(IMutable<T?> mutable) : base(mutable, new IsAssigned<T>(mutable)) {}
}