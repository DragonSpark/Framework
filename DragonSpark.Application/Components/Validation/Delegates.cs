using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Reflection;
using DragonSpark.Reflection.Members;
using System;
using System.Reflection;

namespace DragonSpark.Application.Components.Validation
{
	sealed class Delegates : ISelect<(Type Owner, string Name), Func<object, object>>
	{
		public static Delegates Default { get; } = new Delegates();

		Delegates() : this(PropertyValueDelegates.Default, AllInstanceFlags.Default) {}

		readonly IPropertyValueDelegate _delegates;
		readonly BindingFlags           _flags;

		public Delegates(IPropertyValueDelegate delegates, BindingFlags flags)
		{
			_delegates = delegates;
			_flags     = flags;
		}

		public Func<object, object> Get((Type Owner, string Name) parameter)
		{
			var (owner, name) = parameter;
			var property = owner.GetProperty(name, _flags)
			                    .Verify($"Could not locate property '{name}' on type '{owner}'");
			var result = _delegates.Get(property);
			return result;
		}
	}
}