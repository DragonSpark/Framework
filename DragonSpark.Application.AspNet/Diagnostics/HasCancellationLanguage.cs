using DragonSpark.Model.Selection.Conditions;
using System.Buffers;

namespace DragonSpark.Application.AspNet.Diagnostics;

sealed class HasCancellationLanguage : ICondition<string>
{
	public static HasCancellationLanguage Default { get; } = new();

	HasCancellationLanguage()
		: this("Operation cancelled by user.", "A task was canceled.", "The operation was canceled",
		       "TaskCanceledException", "OperationCanceledException") {}

	public HasCancellationLanguage(params string[] messages)
		: this(SearchValues.Create(messages, StringComparison.OrdinalIgnoreCase)) {}

	readonly SearchValues<string> _search;

	public HasCancellationLanguage(SearchValues<string> search) => _search = search;

	public bool Get(string parameter) => parameter.AsSpan().ContainsAny(_search);
}