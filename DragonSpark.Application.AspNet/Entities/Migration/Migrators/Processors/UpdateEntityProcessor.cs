using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Destination;
using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Source;
using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Processors;

public sealed class UpdateEntityProcessor<TFrom, TTo> : StopAware<SourceInput<TFrom>, uint>, IEntityProcessor<TFrom>
	where TTo : class where TFrom : class
{
	readonly IEntityProcessor<TFrom> _select;

	public UpdateEntityProcessor(IMap map)
		: this(new ExceptionAwareEntityProcessor<TFrom, TTo>(new UpsertEntities<TFrom,TTo>(map))) {}

	public UpdateEntityProcessor(IEntityProcessor<TFrom> select) : base(select) => _select = select;

	public uint? Get() => _select.Get();
}