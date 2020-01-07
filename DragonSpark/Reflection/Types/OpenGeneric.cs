using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using DragonSpark.Runtime;
using System;

namespace DragonSpark.Reflection.Types
{
	public class OpenGeneric : Select<Type, Func<Array<Type>, Func<object>>>
	{
		public OpenGeneric(Type definition)
			: base(new ContainsGenericInterfaceGuard(definition).Then()
			                                                    .ToConfiguration()
			                                                    .Select(x => new Generic<object>(x).ToDelegate())) {}
	}
}