using DragonSpark.Application.AspNet.Entities.Queries.Runtime.Pagination;
using DragonSpark.Application.AspNet.Entities.Queries.Runtime.Shape;
using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using Syncfusion.Blazor;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.SyncfusionRendering.Queries;

public sealed class Body<T> : IBody<T>
{
	public static Body<T> Default { get; } = new();

	Body() : this(BodyQuery<T>.Default) {}

	readonly IQuery<T>                              _body;
	readonly ISelect<PageInput, DataManagerRequest> _select;

	public Body(IQuery<T> body) : this(body, DataManagerRequests.Default) {}

	public Body(IQuery<T> body, ISelect<PageInput, DataManagerRequest> select)
	{
		_body        = body;
		_select = select;
	}

	public async ValueTask<IQueryable<T>> Get(ComposeInput<T> parameter)
	{
		var (input, current) = parameter;
		var (_, result, _)   = await _body.Off(new(_select.Get(input), current));
		return result;
	}
}