using DragonSpark.Compose;
using DragonSpark.Model.Selection;

namespace DragonSpark.Reflection.Members;

public class PropertyAccessor<T> : ISelect<object, T>
{
	readonly IPropertyDelegates<T> _services;
	readonly string                _name;

	protected PropertyAccessor(string name) : this(PropertyDelegates<T>.Default, name) {}

	public PropertyAccessor(IPropertyDelegates<T> services, string name)
	{
		_services = services;
		_name     = name;
	}

	public T Get(object parameter) => _services.Get(parameter.GetType(), _name)(parameter);
}