using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Model.Sequences;
using System;
using System.Reflection;

namespace DragonSpark.Reflection
{
	class AttributesStore<T> : Conditional<ICustomAttributeProvider, Array<T>>, IAttributes<T> where T : Attribute
	{
		public AttributesStore(ICondition<ICustomAttributeProvider> condition,
		                       ISelect<ICustomAttributeProvider, Array<T>> source)
			: base(condition, source.ToTable()) {}
	}
}