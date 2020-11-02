using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Reflection.Members;
using System;

namespace DragonSpark.Application.Components.Validation
{
	sealed class Delegates : ISelect<(Type Owner, string Name), Func<object, object>>
	{
		public static Delegates Default { get; } = new Delegates();

		Delegates() : this(PropertyDelegates.Default) {}

		readonly IPropertyDelegates _delegates;

		public Delegates(IPropertyDelegates delegates) => _delegates = delegates;

		public Func<object, object> Get((Type Owner, string Name) parameter)
		{
			var (owner, name) = parameter;
			var property = owner.GetProperty(name).Verify($"Could not locate property '{name}' on type '{owner}'");
			var result   = _delegates.Get(owner)(property);
			return result;
		}
	}
}