using DragonSpark.Compose;
using DragonSpark.Model.Results;
using DragonSpark.Reflection.Members;
using System;
using System.Reflection;

namespace DragonSpark.Application.Entities.Generation
{
	sealed class LocateAssignment<T, TValue> : IResult<Action<T, TValue>?>
	{
		public static LocateAssignment<T, TValue> Default { get; } = new LocateAssignment<T, TValue>();

		LocateAssignment() : this(LocatePrincipalProperty<T, TValue>.Default) {}

		readonly Func<PropertyInfo?>                    _property;
		readonly IPropertyAssignmentDelegate<T, TValue> _delegates;

		public LocateAssignment(ILocatePrincipalProperty property)
			: this(property.Then().Bind(A.Type<T>()), PropertyAssignmentDelegates<T, TValue>.Default) {}

		public LocateAssignment(Func<PropertyInfo?> property, IPropertyAssignmentDelegate<T, TValue> delegates)
		{
			_property  = property;
			_delegates = delegates;
		}

		public Action<T, TValue>? Get()
		{
			var property = _property();
			var result   = property != null ? _delegates.Get(property) : null;
			return result;
		}
	}
}