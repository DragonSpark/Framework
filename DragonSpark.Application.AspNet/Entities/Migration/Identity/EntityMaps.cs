using DragonSpark.Model.Selection.Stores;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.AspNet.Entities.Migration.Identity;

sealed class EntityMaps<TFrom, TTo> : ReferenceValueStore<DbContext, IEntityMap<TFrom, TTo>>, IEntityMaps<TFrom, TTo>
	where TFrom : class where TTo : class
{
	public static EntityMaps<TFrom, TTo> Default { get; } = new();

	EntityMaps() : base(x => new EntityMap<TFrom, TTo>(x)) {}
}