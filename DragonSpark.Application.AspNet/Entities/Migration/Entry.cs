using DragonSpark.Compose;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DragonSpark.Application.AspNet.Entities.Migration;

public readonly record struct Entry<T>(EntityEntry Subject, T Entity)
{
	public Entry(EntityEntry Subject) : this(Subject, Subject.Entity.To<T>()) {}
}