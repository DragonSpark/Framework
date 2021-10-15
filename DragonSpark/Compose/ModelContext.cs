using DragonSpark.Compose.Extents.Commands;
using DragonSpark.Compose.Extents.Conditions;
using DragonSpark.Compose.Extents.Generics;
using DragonSpark.Compose.Extents.Results;
using DragonSpark.Compose.Extents.Selections;
using System;

namespace DragonSpark.Compose;

public sealed class ModelContext
{
	public static ModelContext Default { get; } = new ModelContext();

	ModelContext() {}

	public ConditionContext Condition => ConditionContext.Default;

	public ResultContext Result => ResultContext.Default;

	public CommandContext Command => CommandContext.Default;

	public SelectionContext Selection => SelectionContext.Default;

	public GenericContext Generic(Type definition) => new GenericContext(definition);
}