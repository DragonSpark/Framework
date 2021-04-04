using DragonSpark.Compose;
using DragonSpark.Model.Selection.Conditions;
using System;
using System.Reflection;

namespace DragonSpark.Reflection
{
	class AttributeStore<T> : Conditional<ICustomAttributeProvider, T?>, IAttribute<T> where T : Attribute
	{
		public AttributeStore(IAttributes<T> attributes)
			: base(attributes.Condition, attributes.Select(x => x.Open().Only().Account()).ToTable()) {}
	}
}