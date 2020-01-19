using DragonSpark.Model.Results;
using DragonSpark.Runtime.Activation;
using System;

namespace DragonSpark.Model.Commands
{
	public class DelegatedInstanceCommand<T> : ICommand<T>, IActivateUsing<IResult<ICommand<T>>>
	{
		readonly Func<ICommand<T>> _instance;

		public DelegatedInstanceCommand(IResult<ICommand<T>> result) : this(result.Get) {}

		public DelegatedInstanceCommand(Func<ICommand<T>> instance) => _instance = instance;

		public void Execute(T parameter) => _instance().Execute(parameter);
	}
}