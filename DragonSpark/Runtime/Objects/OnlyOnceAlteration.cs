using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Runtime.Execution;

namespace DragonSpark.Runtime.Objects;

sealed class OnlyOnceAlteration<TIn, TOut> : IAlteration<ISelect<TIn, TOut>>
{
	public static OnlyOnceAlteration<TIn, TOut> Default { get; } = new OnlyOnceAlteration<TIn, TOut>();

	OnlyOnceAlteration() {}

	public ISelect<TIn, TOut> Get(ISelect<TIn, TOut> parameter)
		=> new ThreadAwareFirst().Then()
		              .Bind()
		              .Accept<TIn>()
		              .Return()
		              .To(parameter.Then().OrDefault)
		              .Get();
}