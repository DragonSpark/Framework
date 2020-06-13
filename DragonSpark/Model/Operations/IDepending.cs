namespace DragonSpark.Model.Operations
{
	public interface IDepending<in T> : ISelecting<T, bool> {}
}