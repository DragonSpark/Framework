using DragonSpark.Model.Selection.Conditions;
using System.Linq;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

sealed class IsIdentityEntity : ICondition<IEntityType>
{
	public static IsIdentityEntity Default { get; } = new();

	IsIdentityEntity() {}

	public bool Get(IEntityType type)
	{
		var key = type.FindPrimaryKey();
		return key is not null &&
		       key.Properties.Any(x => x.GetValueGenerationStrategy() ==
		                               SqlServerValueGenerationStrategy.IdentityColumn);
	}
}