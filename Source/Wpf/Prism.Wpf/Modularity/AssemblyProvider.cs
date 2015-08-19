using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace Prism.Modularity
{
	public class AssemblyProvider : AssemblyProviderBase
	{
		public static AssemblyProvider Instance
		{
			get { return InstanceField; }
		}	static readonly AssemblyProvider InstanceField = new AssemblyProvider();

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