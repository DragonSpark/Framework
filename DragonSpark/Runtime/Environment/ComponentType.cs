using System;
using DragonSpark.Model.Selection;

namespace DragonSpark.Runtime.Environment
{
	sealed class ComponentType : Select<Type, Type>
	{
		public static ComponentType Default { get; } = new ComponentType();

		ComponentType() : base(ComponentTypes.Default.Query().FirstAssigned()) {}
	}
}