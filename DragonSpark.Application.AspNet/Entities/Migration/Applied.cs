using DragonSpark.Model.Selection;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace DragonSpark.Application.AspNet.Entities.Migration;

sealed class Applied : ISelect<ApplyInput, EntityEntry>
{
	public static Applied Default { get; } = new();

	Applied() {}

	public EntityEntry Get(ApplyInput parameter)
	{
		var (context, source) = parameter;

		if (!Equals(source.Context, context))
		{
			var model = context.Model;
			var entityType = model.FindEntityType(source.Entity.GetType()) ?? model.FindEntityType(source.Metadata.Name)
			                 ?? source.Metadata;

			var entry  = context.GetService<IStateManager>().GetOrCreateEntry(source.Entity, entityType);
			var result = context.Attach(entry.ToEntityEntry().Identified(source).Entity);
			return result;
		}

		return source;
	}
}