using DragonSpark.Application.Entities.Queries.Runtime;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Presentation.Components.Content.Sequences;

sealed class PreRenderAwareAny<T> : Validating<IQueries<T>, bool>, IDepending<IQueries<T>>
{
	public PreRenderAwareAny(ICondition condition, IDepending<IQueries<T>> @true,
	                         IDepending<IQueries<T>> @false)
		: this(condition.Then().Bind().Accept<IQueries<T>>().Operation().Out(), @true, @false) {}

	public PreRenderAwareAny(IDepending<IQueries<T>> condition, IDepending<IQueries<T>> @true,
	                         IDepending<IQueries<T>> @false)
		: base(condition, @true, @false) {}
}