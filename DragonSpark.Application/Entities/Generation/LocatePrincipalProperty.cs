using DragonSpark.Compose;
using NetFabric.Hyperlinq;
using System;
using System.Buffers;
using System.Reflection;

namespace DragonSpark.Application.Entities.Generation;

sealed class LocatePrincipalProperty<T, TValue> : LocatePrincipalProperty
{
	public static LocatePrincipalProperty<T, TValue> Default { get; } = new LocatePrincipalProperty<T, TValue>();

	LocatePrincipalProperty() : this(PrincipalProperty<T, TValue>.Default) {}

	public LocatePrincipalProperty(IPrincipalProperty property)
		: this(property, Start.A.Selection<PropertyInfo>()
		                      .By.Calling(x => x.PropertyType)
		                      .Select(Is.EqualTo(A.Type<TValue>()))
		                      .Out()
		                      .Then()) {}

	public LocatePrincipalProperty(IPrincipalProperty property, Func<PropertyInfo, bool> filter)
		: base(property, filter) {}
}

class LocatePrincipalProperty : ILocatePrincipalProperty
{
	readonly IPrincipalProperty       _property;
	readonly Func<PropertyInfo, bool> _filter;
	readonly ArrayPool<PropertyInfo>  _pool;

	public LocatePrincipalProperty(IPrincipalProperty property, Func<PropertyInfo, bool> filter)
		: this(property, filter, ArrayPool<PropertyInfo>.Shared) {}

	public LocatePrincipalProperty(IPrincipalProperty property, Func<PropertyInfo, bool> filter,
	                               ArrayPool<PropertyInfo> pool)
	{
		_property = property;
		_filter   = filter;
		_pool     = pool;
	}

	public PropertyInfo? Get(Type parameter)
	{
		using var owner  = parameter.GetProperties().AsValueEnumerable().Where(_filter).ToArray(_pool);
		var       result = _property.Get(owner.Memory);
		return result;
	}
}