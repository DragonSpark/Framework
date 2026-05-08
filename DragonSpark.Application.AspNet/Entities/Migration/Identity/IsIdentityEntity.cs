using DragonSpark.Model.Selection.Conditions;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Linq;

namespace DragonSpark.Application.AspNet.Entities.Migration.Identity;

sealed class IsIdentityEntity : ICondition<IEntityType>
{
	public static IsIdentityEntity Default { get; } = new();

	IsIdentityEntity() : this(IsIdentityProperty.Default.Get) {}

	readonly Func<IProperty, bool> _identity;

	public IsIdentityEntity(Func<IProperty, bool> identity) => _identity = identity;

	public bool Get(IEntityType type) => type.FindPrimaryKey()?.Properties?.Any(_identity) == true;
}