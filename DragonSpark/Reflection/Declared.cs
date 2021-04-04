using DragonSpark.Compose;
using DragonSpark.Model.Selection.Conditions;
using System;
using System.Reflection;

namespace DragonSpark.Reflection
{
	class Declared<TAttribute, T> : Conditional<ICustomAttributeProvider, T> where TAttribute : Attribute
	{
		protected Declared(Func<TAttribute, T> select) : this(LocalAttribute<TAttribute>.Default, select) {}

		protected Declared(IAttribute<TAttribute> attribute, Func<TAttribute, T> select)
			: base(attribute.Condition,
			       attribute.Select(select.Start().Ensure.Input.IsAssigned.Otherwise.UseDefault().Accounting().Get())
			                .ToTable()) {}
	}
}