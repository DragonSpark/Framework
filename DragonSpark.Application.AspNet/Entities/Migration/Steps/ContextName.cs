using DragonSpark.Model.Selection.Stores;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.AspNet.Entities.Migration.Steps;

sealed class ContextName : ReferenceValueTable<DbContext, string>
{
	public static ContextName Default { get; } = new();

	ContextName() {}
}