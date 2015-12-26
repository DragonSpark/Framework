using System;
using System.Reflection;
using DragonSpark.Extensions;

namespace DragonSpark.Modularity
{
	public static class ModuleInfoExtensions
	{
		public static Assembly GetAssembly( this ModuleInfo target )
		{
			var result = target.ResolveType()?.Assembly();
			return result;
		}

		public static Type ResolveType( this ModuleInfo target )
		{
			var result = Type.GetType( target.ModuleType, true );
			return result;
		}
	}
}