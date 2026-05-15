using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq.Expressions;

namespace DragonSpark.Application.AspNet.Entities.Migration.Identity;

sealed class ComposeKeySelector : ISelect<IEntityType, LambdaExpression>
{
	public static ComposeKeySelector Default { get; } = new();

	ComposeKeySelector() {}

	public LambdaExpression Get(IEntityType parameter)
	{
		var key = parameter.FindPrimaryKey().Verify().Properties;
		var x   = Expression.Parameter(parameter.ClrType, "x");
		return Expression.Lambda(Expression.Property(x, key[0].PropertyInfo.Verify()), x);
	}
}