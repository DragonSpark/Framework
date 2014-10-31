using System;

namespace DragonSpark.Runtime
{
	public interface IFactory
	{
		object Create( Type resultType, object parameter = null );
	}
}