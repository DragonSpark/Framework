namespace DragonSpark.Application.Entities.Queries.Runtime.Materialize;

sealed class DefaultMaterialization<T> : Materialization<T>
{
	public static DefaultMaterialization<T> Default { get; } = new DefaultMaterialization<T>();

	DefaultMaterialization() : base(DefaultAny<T>.Default, Counting<T>.Default, Sequences<T>.Default) {}
}