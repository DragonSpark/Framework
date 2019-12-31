using DragonSpark.Compose.Conditions;
using System;

namespace DragonSpark.Compose
{
	public sealed class ModelContext
	{
		public static ModelContext Default { get; } = new ModelContext();

		ModelContext() {}

		public Context Condition => Context.Default;

		public Results.Context Result => Results.Context.Default;

		public Commands.Context Command => Commands.Context.Default;

		public Selections.Context Selection => Selections.Context.Default;

		public Generics.Context Generic(Type definition) => new Generics.Context(definition);
	}
}