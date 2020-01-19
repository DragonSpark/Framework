using DragonSpark.Compose;
using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Model.Sequences;
using System;

namespace DragonSpark.Runtime.Environment
{
	sealed class ComponentType : Alteration<Type>, IComponentType
	{
		public ComponentType(IArray<Type, Type> select)
			: base(select.Then()
			             .FirstAssigned()
			             .Ensure.Output.IsAssigned.Otherwise.Throw(LocateGuardMessage.Default)) {}
	}
}