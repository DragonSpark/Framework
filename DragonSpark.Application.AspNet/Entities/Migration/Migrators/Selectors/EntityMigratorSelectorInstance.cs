using DragonSpark.Model.Results;
using DragonSpark.Model.Sequences;
using System;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Selectors;

public class EntityMigratorSelectorInstance : Instance<IEntityMigratorSelector>
{
	protected EntityMigratorSelectorInstance(IEntityMigratorSelector start, Array<Type> ignore, Array<Type> exact)
		: base(start.Ignoring(ignore).Exact(exact).WithIdentityAwareness().WithExceptionAwareness()) {}
}