using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Reflection.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

sealed class ConstructExactEntityMigrator : ISelect<ConstructEntityMigratorInput, IEntityMigrator>
{
	public static ConstructExactEntityMigrator Default { get; } = new();

	ConstructExactEntityMigrator()
		: this(Start.A.Generic(typeof(EntityMigrator<,>))
		            .Of.Type<IEntityMigrator>()
		            .WithParameterOf<DbContext>()
		            .AndOf<IModel>(),
		       NamedModels.Default) {}

	readonly IGeneric<DbContext, IModel, IEntityMigrator> _generic;
	readonly ICondition<IEntityType>                      _condition;

	public ConstructExactEntityMigrator(IGeneric<DbContext, IModel, IEntityMigrator> generic,
	                                    ICondition<IEntityType> condition)
	{
		_generic   = generic;
		_condition = condition;
	}

	public IEntityMigrator Get(ConstructEntityMigratorInput parameter)
	{
		var (definition, from, to) = parameter;
		var model = definition.Model;
		return _condition.Get(to)
			       ? new NamedEntityMigrator(from)
			       : _generic.Get(from.ClrType, to.ClrType)(definition.Source, model);
	}
}