using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Azure.Storage.Uploads;

public sealed class IsTemporaryFile : ICondition<string>
{
	readonly string _root;

	public IsTemporaryFile(TemporaryRoot settings) : this(settings.Get()) {}

	public IsTemporaryFile(string root) => _root = root;

	public bool Get(string parameter) => parameter.StartsWith(_root);
}