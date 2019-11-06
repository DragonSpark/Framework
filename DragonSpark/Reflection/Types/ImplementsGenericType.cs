using System;
using System.Reflection;
using DragonSpark.Compose;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Reflection.Types
{
	public class ImplementsGenericType : Condition<TypeInfo>
	{
		public ImplementsGenericType(Type definition) : base(Start.An.Instance(GenericInterfaceImplementations.Default)
		                                                          .Select(x => x.Condition)
		                                                          .Select(definition.To)) {}
	}
}