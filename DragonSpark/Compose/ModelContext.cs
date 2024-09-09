﻿using DragonSpark.Compose.Extents.Commands;
using DragonSpark.Compose.Extents.Conditions;
using DragonSpark.Compose.Extents.Generics;
using DragonSpark.Compose.Extents.Results;
using DragonSpark.Compose.Extents.Selections;
using System;

namespace DragonSpark.Compose;

public sealed class ModelContext
{
	public static ModelContext Default { get; } = new();

	ModelContext() {}

	public ConditionContext Condition => ConditionContext.Default;

	public ResultComposer Result => ResultComposer.Default;

	public CommandComposer Command => CommandComposer.Default;

	public SelectionContext Selection => SelectionContext.Default;

	public GenericContext Generic(Type definition) => new(definition);
}