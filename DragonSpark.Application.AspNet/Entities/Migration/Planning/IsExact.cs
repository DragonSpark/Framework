using DragonSpark.Model.Selection.Conditions;
using System;
using System.Linq;

namespace DragonSpark.Application.AspNet.Entities.Migration.Planning;

sealed class IsExact : ICondition<IsExactInput>
{
	public static IsExact Default { get; } = new();

	IsExact() : this(x => new(x.Name, x.ClrType.IsEnum ? typeof(Enum) : x.ClrType)) {}

	readonly Func<Microsoft.EntityFrameworkCore.Metadata.IProperty, PropertyRecord> _record;

	public IsExact(Func<Microsoft.EntityFrameworkCore.Metadata.IProperty, PropertyRecord> record) => _record = record;

	public bool Get(IsExactInput parameter)
	{
		var (source, destination) = parameter;
		return source.GetProperties()
		             .Select(_record)
		             .ToHashSet()
		             .SetEquals(destination.GetProperties().Select(_record).ToHashSet())
		       && source.GetNavigations()
		                .Select(n => n.Name)
		                .ToHashSet()
		                .SetEquals(destination.GetNavigations().Select(n => n.Name).ToHashSet());
	}
}