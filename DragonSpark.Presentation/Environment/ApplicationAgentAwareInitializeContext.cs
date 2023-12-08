using DragonSpark.Model.Selection.Conditions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Presentation.Environment;

sealed class ApplicationAgentAwareInitializeContext : IInitializeContext
{
	readonly IInitializeContext      _previous;
	readonly IsApplicationAgentStore _store;
	readonly ICondition<HttpContext> _condition;

	[UsedImplicitly]
	public ApplicationAgentAwareInitializeContext(IInitializeContext previous, IsApplicationAgentStore store)
		: this(previous, store, IsApplicationAgent.Default) {}

	public ApplicationAgentAwareInitializeContext(IInitializeContext previous, IsApplicationAgentStore store,
	                                              ICondition<HttpContext> condition)
	{
		_previous  = previous;
		_store     = store;
		_condition = condition;
	}

	public void Execute(HttpContext parameter)
	{
		_previous.Execute(parameter);
		_store.Execute(_condition.Get(parameter));
	}
}