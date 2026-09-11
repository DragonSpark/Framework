using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DragonSpark.Application.AspNet.Entities.Migration;

public readonly record struct LoadInput<T>(EntityEntry Subject, Func<IQueryable<T>, IQueryable<T>> Include)
	where T : class;