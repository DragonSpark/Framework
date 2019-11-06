using System;
using DragonSpark.Model.Selection.Alterations;

namespace DragonSpark.Reflection.Types
{
	sealed class GenericTypeDefinition : Alteration<Type>
	{
		public static GenericTypeDefinition Default { get; } = new GenericTypeDefinition();

		GenericTypeDefinition() : base(x => x.GetGenericTypeDefinition()) {}
	}
}