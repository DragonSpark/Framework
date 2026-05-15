using DragonSpark.Compose;
using DragonSpark.Model.Operations.Selection.Stop;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Model.Selection.Stores;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace DragonSpark.Application.AspNet.Entities.Migration.Identity;

sealed class EntityMap<TFrom, TTo>
	: ReferenceValueStore<DbContext, IStopAware<IReadOnlyCollection<TFrom>, IConditional<object, TTo>>>,
	  IEntityMap<TFrom, TTo>
	where TFrom : class where TTo : class
{
	public EntityMap(DbContext source) : base(x => new ComposeEntityMap<TFrom, TTo>(source, x).AsReferenceStoring()) {}
}