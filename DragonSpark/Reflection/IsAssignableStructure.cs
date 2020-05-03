using DragonSpark.Runtime;
using System;

namespace DragonSpark.Reflection
{
	sealed class IsAssignableStructure : IsAssigned<Type, Type>
	{
		public static IsAssignableStructure Default { get; } = new IsAssignableStructure();

		IsAssignableStructure() : base(Nullable.GetUnderlyingType!) {}
	}
}