namespace DragonSpark.Application.Entities.Queries.Runtime.Shape
{
	public sealed class Pagers<T> : IPagers<T>
	{
		public static Pagers<T> Default { get; } = new Pagers<T>();

		Pagers() {}

		public IPaging<T> Get(PagingInput<T> parameter) => new Paging<T>(parameter.Queries, parameter.Compose);
	}
}