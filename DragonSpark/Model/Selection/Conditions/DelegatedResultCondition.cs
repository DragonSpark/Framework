using System;
using DragonSpark.Runtime;
using DragonSpark.Runtime.Activation;

namespace DragonSpark.Model.Selection.Conditions
{
	public class DelegatedResultCondition : DelegatedResultCondition<None>, ICondition
	{
		public DelegatedResultCondition(Func<bool> @delegate) : base(@delegate) {}
	}

	public class DelegatedResultCondition<T> : ICondition<T>, IActivateUsing<Func<bool>>
	{
		readonly Func<bool> _delegate;

		public DelegatedResultCondition(Func<bool> @delegate) => _delegate = @delegate;

		public bool Get(T _) => _delegate();
	}
}