using System;

namespace DragonSpark.Activation
{
	public interface IServiceRegistry
	{
		void Register( Type from, Type mappedTo );

		void Register( Type type, object instance );

		void RegisterFactory( Type type, Func<object> factory );
	}
}