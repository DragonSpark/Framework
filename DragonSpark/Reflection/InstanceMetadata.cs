using DragonSpark.Compose;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Conditions;
using System;
using System.Reflection;

namespace DragonSpark.Reflection
{
	class InstanceMetadata<TIn, TAttribute, TOut> : Conditional<TIn, TOut> where TAttribute : Attribute, IResult<TOut>
	{
		protected InstanceMetadata() : this(AttributeProvider<TIn>.Default, DeclaredValue<TAttribute, TOut>.Default) {}

		protected InstanceMetadata(ISelect<TIn, ICustomAttributeProvider> select,
		                           IConditional<ICustomAttributeProvider, TOut> value)
			: base(select.Select(value.Condition).Get, select.Select(value.Get).Get) {}
	}
}