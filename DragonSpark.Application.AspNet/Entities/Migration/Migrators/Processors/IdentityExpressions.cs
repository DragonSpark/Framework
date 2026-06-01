using DragonSpark.Model.Selection.Stores;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Processors;

public sealed class IdentityExpressions : ConcurrentTable<IEntityType, string>
{
	public static IdentityExpressions Default { get; } = new();

	IdentityExpressions() {}
}