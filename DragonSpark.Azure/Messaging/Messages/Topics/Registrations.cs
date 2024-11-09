using DragonSpark.Azure.Messaging.Messages.Topics.Receive;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Azure.Messaging.Messages.Topics;

sealed class Registrations : ICommand<IServiceCollection>
{
	public static Registrations Default { get; } = new();

	Registrations() {}

	public void Execute(IServiceCollection parameter)
	{
		parameter.Start<ProcessEvents>().Include(x => x.Dependencies.Recursive()).Singleton();
	}
}