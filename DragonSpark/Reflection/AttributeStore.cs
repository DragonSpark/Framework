using System;
using System.Reflection;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Reflection
{
	class AttributeStore<T> : Conditional<ICustomAttributeProvider, T>, IAttribute<T> where T : Attribute
	{
		public AttributeStore(IAttributes<T> attributes)
			: base(attributes.Condition, attributes.Query().Only().ToTable()) {}
	}
}