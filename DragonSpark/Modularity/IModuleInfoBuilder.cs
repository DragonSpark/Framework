using System;

namespace DragonSpark.Modularity
{
	public interface IModuleInfoBuilder
	{
		ModuleInfo CreateModuleInfo( Type type );
	}
}