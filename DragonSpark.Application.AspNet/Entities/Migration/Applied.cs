using DragonSpark.Model.Selection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DragonSpark.Application.AspNet.Entities.Migration;

sealed class Applied<T> : ISelect<ApplyInput<T>, EntityEntry<T>> where T : class
{
	public static Applied<T> Default { get; } = new();

	Applied() {}

	public EntityEntry<T> Get(ApplyInput<T> parameter)
	{
		var (context, entry) = parameter;

		if (!Equals(entry.Context, context))
		{
			var current = context.Entry(entry.Entity);
			var attach  = current is { State: EntityState.Detached } ? context.Attach(entry.Entity) : current;
			return attach.Assigned(entry);
		}

		return entry;
	}
}