using DragonSpark.Compose;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Queries.Runtime.Shape;

public sealed class EmptyPaging<T> : IPaging<T>
{
	public static EmptyPaging<T> Default { get; } = new();

	EmptyPaging() : this(Current<T>.Default) {}

	readonly Current<T> _instance;

	public EmptyPaging(Current<T> instance) => _instance = instance;

	public ValueTask<Current<T>> Get(QueryInput parameter) => _instance.ToOperation();
}