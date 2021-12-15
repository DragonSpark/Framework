using DragonSpark.Application.Entities.Queries.Runtime.Shape;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Presentation.Components.Content.Sequences;

sealed class PreRenderAwarePaging<T> : Validating<QueryInput, Current<T>>, IPaging<T>
{
	public PreRenderAwarePaging(ICondition condition, IPaging<T> @true, IPaging<T> @false)
		: this(condition.Then().Bind().Accept<QueryInput>().Operation().Out(), @true, @false) {}

	public PreRenderAwarePaging(IDepending<QueryInput> condition, IPaging<T> @true, IPaging<T> @false)
		: base(condition, @true, @false) {}
}