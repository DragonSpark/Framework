using System;
using System.Reflection;

namespace DragonSpark.Modularity
{
	public static class ModuleInfoExtensions
	{
		public static Assembly GetAssembly( this ModuleInfo target )
		{
			var result = Type.GetType( target.ModuleType, true )?.GetTypeInfo().Assembly;
			return result;
		}
	}
}