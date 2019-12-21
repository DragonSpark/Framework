using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Reflection.Types;
using System;

namespace DragonSpark.Runtime
{
	public sealed class AssignableFromGuard : Guard<Type, InvalidOperationException>
	{
		public AssignableFromGuard(Type type) : this(type, new ExpectedTypeMessage(type)) {}

		public AssignableFromGuard(Type type, ISelect<Type, string> message)
			: this(new IsAssignableFrom(type).Then().Inverse().Out(), message) {}

		public AssignableFromGuard(ICondition<Type> condition, ISelect<Type, string> message)
			: base(condition, message) {}
	}
}