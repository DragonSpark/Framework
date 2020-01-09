using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Selection;
using DragonSpark.Reflection.Types;
using System;

namespace DragonSpark.Runtime
{
	public sealed class ContainsGenericInterfaceGuard : Guard<Type, InvalidOperationException>
	{
		public ContainsGenericInterfaceGuard(Type type) : this(type, new ExpectedTypeMessage(type)) {}

		public ContainsGenericInterfaceGuard(Type type, ISelect<Type, string> message)
			: this(new ContainsGenericInterface(type).Then().Inverse().Get().Get, message.Get) {}

		public ContainsGenericInterfaceGuard(Func<Type, bool> condition, Func<Type, string> message)
			: base(condition, message) {}
	}
}