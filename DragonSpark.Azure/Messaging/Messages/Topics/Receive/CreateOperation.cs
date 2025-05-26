using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Stop;
using DragonSpark.Model.Selection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Messaging.Messages.Topics.Receive;

sealed class CreateOperation<T> : ISelect<Func<Stop<T>, Task>, IStopAware<object>>
{
	public static CreateOperation<T> Default { get; } = new();

	CreateOperation() {}

	public IStopAware<object> Get(Func<Stop<T>, Task> parameter)
		=> new Process<T>(parameter.Start().Out().Then().Structure().Out(), parameter.Target.Verify().GetType());
}

sealed class CreateOperation<T, U> : ISelect<Func<Stop<T>, Task>, IStopAware<object>> where U : Message<T>
{
	public static CreateOperation<T, U> Default { get; } = new();

	CreateOperation() {}

	public IStopAware<object> Get(Func<Stop<T>, Task> parameter)
	{
		var body = Start.A.Selection<Stop<U>>()
		                .By.Calling(x => x.Subject.Subject.Stop(x))
		                .Select(parameter)
		                .Out()
		                .Then()
		                .Structure()
		                .Out();
		var result = new Process<U>(body, parameter.Target.Verify().GetType());
		return result;
	}
}