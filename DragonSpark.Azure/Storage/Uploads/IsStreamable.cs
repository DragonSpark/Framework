using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Azure.Storage.Uploads;

public sealed class IsStreamable : ICondition<string>
{
	public static IsStreamable Default { get; } = new();

	IsStreamable() {}

	public bool Get(string parameter) => parameter.StartsWith("audio") || parameter.StartsWith("video");
}