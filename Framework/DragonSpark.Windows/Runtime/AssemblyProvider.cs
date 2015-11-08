using System.Linq;
using System.Reflection;
using DragonSpark.Runtime;
using Microsoft.Practices.Unity;

namespace DragonSpark.Windows.Runtime
{
	public class AssemblyProvider : AssemblyProviderBase
	{
		public static AssemblyProvider Instance { get; } = new AssemblyProvider();

		protected override Assembly[] DetermineAll()
		{
			var result = AllClasses.FromAssembliesInBasePath( includeUnityAssemblies: true )
				.Where( x => x.Namespace != null )
				.GroupBy( type => type.Assembly )
				.Select( types => types.Key ).ToArray();
			return result;
		}
	}
}