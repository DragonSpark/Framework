namespace DragonSpark.Application.Entities.Queries.Composition;

public sealed class Set<T> : Query<T> where T : class
{
	public static Set<T> Default { get; } = new Set<T>();

	Set() : base(x => x.Set<T>()) {}
}

public class Set<TIn, T> : Query<TIn, T> where T : class
{
	public static Set<TIn, T> Default { get; } = new();

	Set() : base(x => x.Set<T>()) {}
}