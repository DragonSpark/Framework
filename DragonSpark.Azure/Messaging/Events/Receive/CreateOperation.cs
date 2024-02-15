using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Messaging.Events.Receive;

sealed class CreateOperation<T> : ISelect<Func<T, Task>, IOperation<object>>
{
	public static CreateOperation<T> Default { get; } = new();

	CreateOperation() {}

	public IOperation<object> Get(Func<T, Task> parameter)
		=> new Process<T>(parameter.Start().Out().Then().Structure().Out(), parameter.Target.Verify().GetType());
}

sealed class CreateOperation<T, U> : ISelect<Func<T, Task>, IOperation<object>> where U : Message<T>
{
	public static CreateOperation<T, U> Default { get; } = new();

	CreateOperation() {}

	public IOperation<object> Get(Func<T, Task> parameter)
	{
		var body   = Start.A.Selection<U>().By.Calling(x => x.Subject).Select(parameter).Out().Then().Structure().Out();
		var result = new Process<U>(body, parameter.Target.Verify().GetType());
		return result;
	}
}