using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Runtime.Execution;

namespace DragonSpark.Runtime.Objects;

sealed class OncePerParameter<TIn, TOut> : IAlteration<ISelect<TIn, TOut>> where TIn : notnull
{
	public static OncePerParameter<TIn, TOut> Default { get; } = new();

	OncePerParameter() {}

	public ISelect<TIn, TOut> Get(ISelect<TIn, TOut> parameter)
		=> parameter.Then().OrDefault(new ThreadAwareFirst<TIn>()).Get();
}