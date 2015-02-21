using System;
using System.Collections.Generic;
using System.Reflection;

namespace DragonSpark.Common
{
	public class AssemblyLocator : IAssemblyLocator
	{
		public IEnumerable<Assembly> GetAllAssemblies()
		{
			return AppDomain.CurrentDomain.GetAssemblies();
		}
	}
}