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

	ConstructExactEntityMigrator() : this(Start.A.Generic(typeof(EntityMigrator<,>))
	                                           .Of.Type<IEntityMigrator>()
	                                           .WithParameterOf<DbContext>()
	                                           .AndOf<DbContext>(),
	                                      Start.A.Generic(typeof(EntityMigrator<,>))
	                                           .Of.Type<IEntityMigrator>()
	                                           .WithParameterOf<DbContext>()
	                                           .AndOf<DbContext>()
	                                           .AndOf<string>(), NamedModels.Default) {}

	readonly IGeneric<DbContext, DbContext, IEntityMigrator>         _generic;
	readonly IGeneric<DbContext, DbContext, string, IEntityMigrator> _named;
	readonly ICondition<IEntityType>                                 _condition;

	public ConstructExactEntityMigrator(IGeneric<DbContext, DbContext, IEntityMigrator> generic,
	                                    IGeneric<DbContext, DbContext, string, IEntityMigrator> named,
	                                    ICondition<IEntityType> condition)
	{
		_generic   = generic;
		_named     = named;
		_condition = condition;
	}

	public IEntityMigrator Get(ConstructEntityMigratorInput parameter)
	{
		var (source, destination, from, to) = parameter;
		return _condition.Get(to)
			       ? _named.Get(from.ClrType, to.ClrType)(source, destination, to.Name)
			       : _generic.Get(from.ClrType, to.ClrType)(source, destination);
	}
}