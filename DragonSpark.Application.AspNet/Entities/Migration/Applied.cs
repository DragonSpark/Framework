namespace DragonSpark.Application.AspNet.Entities.Migration;

using DragonSpark.Model.Selection;
using Microsoft.EntityFrameworkCore.ChangeTracking;

sealed class Applied : ISelect<ApplyInput, EntityEntry>
{
	public static Applied Default { get; } = new();

	Applied() {}

	public EntityEntry Get(ApplyInput parameter)
	{
		var (context, source) = parameter;
		return Equals(source.Context, context) ? source : context.Attach(source.Entity).Identified(source);
	}
}