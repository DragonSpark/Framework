using System;
using System.Collections.Generic;

namespace Prism.Modularity
{
	public interface IModuleInfoProvider : IDisposable
	{
		IEnumerable<ModuleInfo> GetModuleInfos();
	}
}