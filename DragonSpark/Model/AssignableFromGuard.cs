using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Reflection.Types;
using System;

namespace DragonSpark.Model
{
	public sealed class AssignableFromGuard : Guard<Type, InvalidOperationException>
	{
		public AssignableFromGuard(Type type) : this(type, new ExpectedTypeMessage(type)) {}

		public AssignableFromGuard(Type type, ISelect<Type, string> message)
			: this(new IsAssignableFrom(type).Then().Inverse().Out(), message.Get) {}

		public AssignableFromGuard(ICondition<Type> condition, Func<Type, string> message)
			: base(condition, message) {}
	}
}