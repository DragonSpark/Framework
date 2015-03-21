using Microsoft.Practices.Unity;
using Prism.Modularity;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Prism.Unity.Windows
{
	public class AssemblyProvider : IAssemblyProvider
	{
		public IEnumerable<Assembly> GetAssemblies()
		{
			var result = AllClasses.FromAssembliesInBasePath( includeUnityAssemblies: true )
				.Where( x => x.Namespace != null )
				.GroupBy( type => type.Assembly )
				.Select( types => types.Key ).ToArray();
			return result;
		}
	}
}