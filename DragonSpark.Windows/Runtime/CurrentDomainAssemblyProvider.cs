using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using DragonSpark.Runtime;
using DragonSpark.TypeSystem;

namespace DragonSpark.Windows.Runtime
{
	public class CurrentDomainAssemblyProvider : AssemblyProviderBase
	{
		public static CurrentDomainAssemblyProvider Instance { get; } = new CurrentDomainAssemblyProvider();

		protected override Assembly[] DetermineAll()
		{
			var query = from Assembly assembly in AppDomain.CurrentDomain.GetAssemblies()
				where !( assembly is AssemblyBuilder )
					  && assembly.GetType().FullName != "System.Reflection.Emit.InternalAssemblyBuilder"
					  && !string.IsNullOrEmpty( assembly.Location )
				orderby assembly.GetName().Name
				select assembly;
			var result = query.ToArray();
			return result;
		}
	}
}