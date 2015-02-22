using System;

namespace DragonSpark.Activation
{
	public interface IFactory
	{
		object Create( Type resultType, object parameter = null );
	}
}