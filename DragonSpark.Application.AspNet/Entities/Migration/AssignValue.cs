using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.AspNet.Entities.Migration;

sealed class AssignValue : IAssignValue
{
	public static AssignValue Default { get; } = new();

	AssignValue() {}

	public void Execute(AssignValueInput parameter)
	{
		var (source, destination) = parameter;

		var metadata    = destination.Metadata;
		var entityEntry = destination.EntityEntry;

		if (entityEntry.State == EntityState.Detached || (!metadata.IsKey() && !metadata.IsForeignKey()))
		{
			destination.CurrentValue = source;
		}
	}
}