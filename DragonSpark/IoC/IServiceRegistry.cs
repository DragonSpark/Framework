using System;

namespace DragonSpark.IoC
{
	public interface IServiceRegistry
	{
		void Register( Type from, Type to );

		void Register( Type type, object instance );

		void RegisterFactory( Type type, Func<object> factory );
	}
}