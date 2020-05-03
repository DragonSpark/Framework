using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Selection.Conditions;
using JetBrains.Annotations;

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

	public sealed class First<T> : Condition<T> where T : notnull
	{
		public First() : base(Start.A.Selection<T>()
		                           .AndOf<First>()
		                           .By.Activation()
		                           .Get()
		                           .ToTable()
		                           .Select(ConditionSelector.Default)) {}
	}
}