using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using System;

namespace DragonSpark.Reflection.Types
{
	sealed class GenericArguments : Select<Type, Array<Type>>
	{
		public static GenericArguments Default { get; } = new GenericArguments();

		GenericArguments() : base(x => x.GenericTypeArguments) {}
	}
}