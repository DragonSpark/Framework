using DragonSpark.Application.Entities.Queries.Runtime.Shape;
using DragonSpark.Compose;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.SyncfusionRendering.Queries;

sealed class Body<T> : IBody<T>
{
	public static Body<T> Default { get; } = new Body<T>();

	Body() : this(BodyQuery<T>.Default) {}

	readonly IQuery<T> _body;

	public Body(IQuery<T> body) => _body = body;

	public async ValueTask<IQueryable<T>> Get(ComposeInput<T> parameter)
	{
		var (input, current) = parameter;
		var (_, result, _)   = await _body.Await(new(input.To<SyncfusionQueryInput>().Request, current));
		return result;
	}
}