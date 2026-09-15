using DragonSpark.Model.Selection.Conditions;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DragonSpark.Application.AspNet.Entities.Migration;

sealed class IdentityProperty : ICondition<IProperty>
{
	public static IdentityProperty Default { get; } = new();

	IdentityProperty() {}

	public bool Get(IProperty parameter)
		=> parameter.IsPrimaryKey() || parameter.IsForeignKey() ||
		   (parameter.IsShadowProperty() && parameter.Name != parameter.DeclaringType.GetDiscriminatorPropertyName());
}