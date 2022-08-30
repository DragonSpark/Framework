using DragonSpark.Model.Results;

namespace DragonSpark.SyncfusionRendering.Entities;

public readonly record struct Updated<T>(T Subject, string Action);

public sealed class Allow<T> : Variable<bool>
{
	public Allow(T subject) : base(true) => Subject = subject;

	public T Subject { get; }
}