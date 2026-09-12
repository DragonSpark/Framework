using DragonSpark.Model.Selection.Conditions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DragonSpark.Application.AspNet.Entities.Migration.Planning;

sealed class MigrationEntity : Condition<IEntityType>
{
	public static MigrationEntity Default { get; } = new();

	MigrationEntity() : base(x => x.IsOwned() && x.GetViewName() is null && x.FindPrimaryKey() != null) {}
}