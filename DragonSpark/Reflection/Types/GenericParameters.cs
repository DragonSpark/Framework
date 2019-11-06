using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using System;
using System.Reflection;

namespace DragonSpark.Reflection.Types
{
	sealed class GenericParameters : Select<TypeInfo, Array<Type>>
	{
		public static GenericParameters Default { get; } = new GenericParameters();

		GenericParameters() : base(x => x.GenericTypeParameters) {}
	}
}