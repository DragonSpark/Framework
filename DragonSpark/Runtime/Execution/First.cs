using JetBrains.Annotations;
using DragonSpark.Compose;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Runtime.Execution
{
	public sealed class First : ICondition
	{
		readonly ICounter _counter;

		[UsedImplicitly]
		public First() : this(new Counter()) {}

		public First(ICounter counter) => _counter = counter;

		public bool Get(None parameter) => _counter.Get() == 0 && _counter.Count() == 1;
	}

	sealed class First<T> : Condition<T>
	{
		public First() : base(Start.A.Selection<T>()
		                           .AndOf<First>()
		                           .By.Activation()
		                           .ToTable()
		                           .Select(ConditionSelector.Default)) {}
	}
}