using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Model.Sequences;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;

namespace DragonSpark.Application.AspNet.Entities.Migration.Identity;

sealed class IsIdentityProperty : ICondition<IProperty>
{
	public static IsIdentityProperty Default { get; } = new();

	IsIdentityProperty() : this(IdentityTypes.Default, IdentityNames.Default) {}

	readonly Array<Type>   _types;
	readonly Array<string> _names;

	public IsIdentityProperty(Array<Type> types, Array<string> names)
	{
		_types = types;
		_names = names;
	}

	public bool Get(IProperty parameter)
		=> parameter.GetValueGenerationStrategy() == SqlServerValueGenerationStrategy.IdentityColumn ||
		   (_types.Open().Contains(parameter.ClrType) && _names.Open().Contains(parameter.Name));
}