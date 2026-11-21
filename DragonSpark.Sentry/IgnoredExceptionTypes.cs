using DragonSpark.Model.Results;
using System.Buffers;

namespace DragonSpark.Sentry;

sealed class IgnoredExceptionTypes : Instance<SearchValues<string>>
{
	public static IgnoredExceptionTypes Default { get; } = new();

	IgnoredExceptionTypes()
		: this("System.Threading.Tasks.TaskCanceledException", "System.OperationCanceledException") {}

	public IgnoredExceptionTypes(params ReadOnlySpan<string> types)
		: base(SearchValues.Create(types, StringComparison.OrdinalIgnoreCase)) {}
}