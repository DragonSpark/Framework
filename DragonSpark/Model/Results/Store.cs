using DragonSpark.Compose;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Runtime.Activation;
using JetBrains.Annotations;

namespace DragonSpark.Model.Results
{
	public class Store<T> : Mutable<T>, IStore<T>, IActivateUsing<IMutable<T>>
	{
		[UsedImplicitly]
		public Store(IMutable<T> mutable) : this(A.Result(mutable)
		                                          .Then()
		                                          .Select(Is.Assigned<T>())
		                                          .Accept()
		                                          .Out(),
		                                         mutable) {}

		public Store(ICondition<None> condition, IMutable<T> mutable) : base(mutable) => Condition = condition;

		public ICondition<None> Condition { get; }
	}
}