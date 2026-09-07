using DragonSpark.Model.Selection.Stores;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DragonSpark.Application.AspNet.Entities.Migration.Steps;

sealed class ContextName : ReferenceValueTable<IModel, string>
{
	public static ContextName Default { get; } = new();

	ContextName() {}
}