using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Grok.Chat;

sealed class LocalRegistrations : ICommand<IServiceCollection>
{
	public static LocalRegistrations Default { get; } = new();

	LocalRegistrations() {}

	public void Execute(IServiceCollection parameter)
	{
		parameter.Start<IChat>()
		         .Forward<Chat>()
		         .Include(x => x.Dependencies)
		         .Singleton()
		         //
		         .Then.Start<IExecuteTools>()
		         .Forward<ExecuteTools>()
		         .Singleton()
		         //
		         .Then.Start<IChatResponse>()
		         .Forward<ChatResponse>()
		         .Decorate<ExceptionAwareChatResponse>()
		         .Include(x => x.Dependencies)
		         .Singleton()
		         //
		         .Then.Start<IToolRegistration>()
		         .Forward<SuggestionToolRegistration>()
		         .Singleton()
		         ;
	}
}