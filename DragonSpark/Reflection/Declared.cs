using System;
using System.Reflection;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Reflection
{
	class Declared<TAttribute, T> : Conditional<ICustomAttributeProvider, T> where TAttribute : Attribute
	{
		protected Declared(Func<TAttribute, T> select) : this(LocalAttribute<TAttribute>.Default, select) {}

		protected Declared(IAttribute<TAttribute> attribute, Func<TAttribute, T> select)
			: base(attribute.Condition, attribute.Select(select).ToTable()) {}
	}
}