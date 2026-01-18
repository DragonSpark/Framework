using DragonSpark.Compose;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

public readonly record struct MapInput(EntityEntry From, EntityEntry To)
{
	public static MapInput New<T>(EntityEntry from, DbContext to) where T : class => new(from, to.Entry(A.New<T>()));
}