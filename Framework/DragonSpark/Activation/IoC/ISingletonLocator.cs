using System;

namespace DragonSpark.Activation.IoC
{
	public interface ISingletonLocator
	{
		object Locate( Type type );
	}
}