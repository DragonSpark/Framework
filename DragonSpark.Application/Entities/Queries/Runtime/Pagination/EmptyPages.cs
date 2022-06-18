using DragonSpark.Compose;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Queries.Runtime.Pagination;

public sealed class EmptyPages<T> : IPages<T>
{
	public static EmptyPages<T> Default { get; } = new();

	EmptyPages() : this(Page<T>.Default) {}

	readonly Page<T> _instance;

	public EmptyPages(Page<T> instance) => _instance = instance;

	public ValueTask<Page<T>> Get(PageInput parameter) => _instance.ToOperation();
}