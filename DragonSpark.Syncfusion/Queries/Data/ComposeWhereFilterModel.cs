using DragonSpark.Compose;
using DragonSpark.Contracts.Queries;
using DragonSpark.Model.Selection;

namespace DragonSpark.SyncfusionRendering.Queries.Data;

sealed class ComposeWhereFilterModel : ISelect<WhereFilter, Syncfusion.Blazor.Data.WhereFilter>
{
	public static ComposeWhereFilterModel Default { get; } = new();

	ComposeWhereFilterModel() {}

	public Syncfusion.Blazor.Data.WhereFilter Get(WhereFilter parameter)
	{
		var (field, ignoreCase, ignoreAccent, isComplex, @operator, condition, value, predicates) = parameter;

		return new()
		{
			Field        = field,
			IgnoreCase   = ignoreCase,
			IgnoreAccent = ignoreAccent,
			IsComplex    = isComplex,
			Operator     = @operator,
			Condition    = condition,
			value        = value,
			predicates
				= predicates.Account()?.Select(Get).ToList() ?? Model.Empty.List<Syncfusion.Blazor.Data.WhereFilter>()
		};
	}
}