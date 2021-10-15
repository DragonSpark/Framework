using DragonSpark.Compose;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Queries.Runtime.Shape;

public sealed class EmptyPaging<T> : IPaging<T>
{
	public static EmptyPaging<T> Default { get; } = new();

	EmptyPaging() {}

	public ValueTask<Current<T>> Get(QueryInput parameter) => Current<T>.Default.ToOperation();
}