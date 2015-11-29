using System;

namespace DragonSpark.Activation.IoC
{
	interface IResolutionSupport
	{
		bool CanResolve( Type type, string name, params object[] parameters );
	}
}