using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Application.Runtime
{
	public class ScopedVariable<T> : Mutable<T?>, IScopedVariable<T?>
	{
		protected ScopedVariable(string key, IScopedTable store)
			: this(new ScopedVariableStore<T>(key, store), store.Condition.Then().Bind(key).Out(),
			       new RemoveScopedVariable(key, store)) {}

		protected ScopedVariable(IMutable<T?> variable, ICondition<None> condition, ICommand remove) : base(variable)
		{
			Condition = condition;
			Remove    = remove;
		}

		public ICondition<None> Condition { get; }
		public ICommand Remove { get; }
	}
}