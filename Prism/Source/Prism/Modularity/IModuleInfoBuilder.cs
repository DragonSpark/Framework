using System;

namespace Prism.Modularity
{
	public interface IModuleInfoBuilder
	{
		ModuleInfo CreateModuleInfo( Type type );
	}
}