using DragonSpark.Application.Entities.Queries.Runtime.Shape;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Stores;
using Syncfusion.Blazor;
using Syncfusion.Blazor.Data;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Syncfusion.Queries
{
	class Class1 {}

	public sealed class SyncfusionQueryInput : QueryInput
	{
		public DataManagerRequest Request { get; set; } = default!;
	}

	sealed class BodyQuery<T> : Alterings<Parameter<T>>, IQuery<T>
	{
		public static BodyQuery<T> Default { get; } = new BodyQuery<T>();

		BodyQuery() : base(Search<T>.Default, Where<T>.Default, Sort<T>.Default) {}
	}

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

	public sealed class SyncfusionCompose<T> : Compose<T>
	{
		public static SyncfusionCompose<T> Default { get; } = new SyncfusionCompose<T>();

		SyncfusionCompose() : base(Body<T>.Default) {}
	}

	sealed class SelectQueryInput : ISelect<DataManagerRequest, SyncfusionQueryInput>
	{
		public static SelectQueryInput Default { get; } = new SelectQueryInput();

		SelectQueryInput() {}

		public SyncfusionQueryInput Get(DataManagerRequest parameter)
			=> new()
			{
				Request = parameter,
				Partition = parameter.Skip > 0 || parameter.Take > 0
					            ? new Partition(parameter.Skip > 0 ? parameter.Skip : null,
					                            parameter.Take > 0 ? parameter.Take : null)
					            : null,
				IncludeTotalCount = parameter.RequiresCounts
			};
	}

	public interface IDataRequests<T> : ISelect<IPaging<T>, IDataRequest>
	{
		
	}

	public sealed class DataRequests<T> : ReferenceValueStore<IPaging<T>, IDataRequest>, IDataRequests<T>
	{
		public static DataRequests<T> Default { get; } = new DataRequests<T>();

		DataRequests() : base(x => new ProcessRequest<T>(x)) {}
	}

	public interface IDataRequest : ISelecting<DataManagerRequest, object> {}

	public sealed class ProcessRequest<T> : IDataRequest
	{
		readonly Await<DataManagerRequest, Current<T>> _current;

		public ProcessRequest(IPaging<T> paging) : this(SelectQueryInput.Default.Then().Select(paging).Then()) {}

		public ProcessRequest(Await<DataManagerRequest, Current<T>> current) => _current = current;

		public async ValueTask<object> Get(DataManagerRequest parameter)
		{
			var evaluate = await _current(parameter);
			var result = evaluate.Total.HasValue
				             ? new DataResult { Result = evaluate, Count = evaluate.Total.Value.Degrade() }
				             : (object)evaluate;
			return result;
		}
	}
}