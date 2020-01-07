using DragonSpark.Compose.Extents.Conditions;
using System;

namespace DragonSpark.Compose
{
	public sealed class ModelContext
	{
		public static ModelContext Default { get; } = new ModelContext();

		ModelContext() {}

		public Context Condition => Context.Default;

		public Extents.Results.Context Result => Extents.Results.Context.Default;

		public Extents.Commands.Context Command => Extents.Commands.Context.Default;

		public Extents.Selections.Context Selection => Extents.Selections.Context.Default;

		public Extents.Generics.Context Generic(Type definition) => new Extents.Generics.Context(definition);
	}
}