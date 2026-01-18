using DragonSpark.Model.Selection.Conditions;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

sealed class IsIdentityEntity : ICondition<IEntityType>
{
	public static IsIdentityEntity Default { get; } = new();

	IsIdentityEntity() {}

	public bool Get(IEntityType type)
		=> type.FindPrimaryKey()?.Properties.Any(p => p.ValueGenerated == ValueGenerated.OnAdd) == true;
}