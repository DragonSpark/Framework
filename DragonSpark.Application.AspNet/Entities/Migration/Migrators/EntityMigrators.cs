using DragonSpark.Application.AspNet.Entities.Migration.Planning;
using DragonSpark.Model.Results;
using DragonSpark.Model.Sequences;
using DragonSpark.Runtime;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

public class EntityMigrators : IEntityMigrators
{
	readonly IArray<IModel, IEntityType>     _order;
	readonly IComposeEntityComparisonResults _results;
	readonly IEntityMigratorSelector         _selector;

	protected EntityMigrators(IModelTypes types, IResult<IEntityMigratorSelector> selector)
		: this(types, selector.Get()) {}

	protected EntityMigrators(IModelTypes types, IEntityMigratorSelector selector)
		: this(MigrationOrder.Default, new ComposeEntityComparisonResults(types), selector) {}

	protected EntityMigrators(IArray<IModel, IEntityType> order, IComposeEntityComparisonResults results,
	                          IEntityMigratorSelector selector)
	{
		_order    = order;
		_results  = results;
		_selector = selector;
	}

	public Array<IEntityMigrator> Get(MigrationInput parameter)
	{
		var (source, destination) = parameter;
		var       order   = _order.Get(source.Model);
		using var results = _results.Get(new(order, destination.Model));
		using var result  = ArrayBuilder.New<IEntityMigrator>(results.Length);
		foreach (var item in results)
		{
			var migrator = _selector.Get(new(source, destination, item));
			if (migrator is not null)
			{
				result.UncheckedAdd(migrator);
			}
		}

		return result;
	}
}