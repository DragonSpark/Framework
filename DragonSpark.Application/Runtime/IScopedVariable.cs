using DragonSpark.Model.Commands;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Application.Runtime
{
	public interface IScopedVariable<T> : IMutable<T?>, IConditionAware
	{
		ICommand Remove { get; }
	}
}