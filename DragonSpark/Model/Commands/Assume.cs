using DragonSpark.Model.Results;
using DragonSpark.Runtime.Activation;
using System;

namespace DragonSpark.Model.Commands;

public class Assume : Assume<None>, ICommand
{
	public Assume(IResult<ICommand> result) : base(result) {}

	public Assume(Func<ICommand> instance) : base(instance) {}
}


public class Assume<T> : ICommand<T>, IActivateUsing<IResult<ICommand<T>>>
{
	readonly Func<ICommand<T>> _instance;

	public Assume(IResult<ICommand<T>> result) : this(result.Get) {}

	public Assume(Func<ICommand<T>> instance) => _instance = instance;

	public void Execute(T parameter)
	{
		_instance().Execute(parameter);
	}
}