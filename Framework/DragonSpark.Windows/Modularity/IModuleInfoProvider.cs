using System;
using System.Collections.Generic;
using DragonSpark.Modularity;

namespace DragonSpark.Windows.Modularity
{
	public interface IModuleInfoProvider : IDisposable
	{
		IEnumerable<ModuleInfo> GetModuleInfos();
	}
}