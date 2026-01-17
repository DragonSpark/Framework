using DragonSpark.Model.Selection.Alterations;
using System;

namespace DragonSpark.Application.AspNet.Entities.Migration.Planning.Comparison;

sealed class DetermineMetadata : IAlteration<Type>
{
	public static DetermineMetadata Default { get; } = new();

	DetermineMetadata() : this(typeof(Enum)) {}

	readonly Type _type;

	public DetermineMetadata(Type type) => _type = type;

	public Type Get(Type parameter) => parameter.IsEnum ? _type : parameter;
}