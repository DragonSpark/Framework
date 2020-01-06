using DragonSpark.Compose;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Runtime;
using DragonSpark.Runtime.Activation;
using JetBrains.Annotations;

namespace DragonSpark.Model.Results
{
	public class Store<T> : Mutable<T>, IStore<T>, IActivateUsing<IMutable<T>>
	{
		[UsedImplicitly]
		public Store(IMutable<T> mutable) : this(mutable.Select(Is.Assigned<T>()).ToSelect().Then().Out(), mutable) {}

		public Store(ICondition<None> condition, IMutable<T> mutable) : base(mutable) => Condition = condition;

		public ICondition<None> Condition { get; }
	}
}