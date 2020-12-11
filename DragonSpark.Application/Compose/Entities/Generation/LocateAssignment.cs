using DragonSpark.Compose;
using DragonSpark.Model.Results;
using DragonSpark.Reflection.Members;
using System;

namespace DragonSpark.Application.Compose.Entities.Generation
{
	sealed class LocateAssignment<T, TValue> : IResult<Action<T, TValue>?>
	{
		public static LocateAssignment<T, TValue> Default { get; } = new LocateAssignment<T, TValue>();

		LocateAssignment() : this(LocatePrincipalProperty<T, TValue>.Default) {}

		readonly ILocatePrincipalProperty               _property;
		readonly IPropertyAssignmentDelegate<T, TValue> _delegates;

		public LocateAssignment(ILocatePrincipalProperty property)
			: this(property, PropertyAssignmentDelegates<T, TValue>.Default) {}

		public LocateAssignment(ILocatePrincipalProperty property, IPropertyAssignmentDelegate<T, TValue> delegates)
		{
			_property  = property;
			_delegates = delegates;
		}

		public Action<T, TValue>? Get()
		{
			var propertyInfo = _property.Get(A.Type<T>());
			var result       = propertyInfo != null ? _delegates.Get(propertyInfo) : null;
			return result;
		}
	}
}