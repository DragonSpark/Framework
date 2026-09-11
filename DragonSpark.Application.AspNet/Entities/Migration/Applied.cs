namespace DragonSpark.Application.AspNet.Entities.Migration;

using DragonSpark.Model.Selection;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;

sealed class Applied : ISelect<ApplyInput, EntityEntry>
{
	public static Applied Default { get; } = new();

	Applied() {}

	public EntityEntry Get(ApplyInput parameter)
	{
		var (context, source) = parameter;

		if (!Equals(source.Context, context))
		{
			var entityType = context.Model.FindEntityType(source.Metadata.Name)
			                 ?? context.Model.FindEntityType(source.Entity.GetType())
			                 ?? source.Metadata;
			var @internal  = context.GetService<IStateManager>().GetOrCreateEntry(source.Entity, entityType);
			var entry = @internal.ToEntityEntry();
			var result = context.Attach(entry.Identified(source).Entity);
			/*if (entry.EntityState == EntityState.Detached)
			{
				entry.SetEntityState(EntityState.Unchanged);
			}*/

			return result;
		}

		return source;
	}
}