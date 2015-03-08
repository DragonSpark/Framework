using System;
using System.Reflection;
using DragonSpark.Extensions;
using Microsoft.Practices.Prism.Modularity;

namespace DragonSpark.IoC
{
	public static class ModuleInfoExtensions
	{
		public static Assembly GetAssembly( this ModuleInfo target )
		{
			var result = Type.GetType( target.ModuleType, false, true ).Transform( x => x.Assembly );
			return result;
		}
	}
}