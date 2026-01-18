using DragonSpark.Model.Results;
using System;
using System.Collections.Generic;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

public class BatchSelectorRegistration<T> : Instance<KeyValuePair<Type, IEntityMigrator>>
{
	protected BatchSelectorRegistration(IEntityMigrator instance) : base(new(typeof(T), instance)) {}
}