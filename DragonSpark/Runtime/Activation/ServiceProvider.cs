using DragonSpark.Compose;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Model.Sequences;
using System;

namespace DragonSpark.Runtime.Activation
{
	public class ServiceProvider : IServiceProvider, ICondition<Type>
	{
		ServiceProvider(Array<object> services, uint length)
		{
			_services = services;
			_length   = length;
		}

		readonly uint          _length;
		readonly Array<object> _services;

		public ServiceProvider(params object[] services) : this(services.Result()) {}

		public ServiceProvider(Array<object> services) : this(services, services.Length) {}

		public bool Get(Type parameter)
		{
			for (var i = 0; i < _length; i++)
			{
				if (parameter.IsInstanceOfType(_services[i]))
				{
					return true;
				}
			}

			return false;
		}

		public object? GetService(Type serviceType)
		{
			for (var i = 0; i < _length; i++)
			{
				var item = _services[i];
				if (serviceType.IsInstanceOfType(item))
				{
					return item;
				}
			}

			return default;
		}
	}
}