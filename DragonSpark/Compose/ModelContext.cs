using DragonSpark.Compose.Extents.Conditions;
using System;

namespace DragonSpark.Compose
{
	public sealed class ModelContext
	{
		public static ModelContext Default { get; } = new ModelContext();

		ModelContext() {}

		public ConditionContext Condition => ConditionContext.Default;

		public Extents.Results.Context Result => Extents.Results.Context.Default;

		public Extents.Commands.CommandContext Command => Extents.Commands.CommandContext.Default;

		public Extents.Selections.Context Selection => Extents.Selections.Context.Default;

		public Extents.Generics.GenericContext Generic(Type definition) => new Extents.Generics.GenericContext(definition);
	}
}