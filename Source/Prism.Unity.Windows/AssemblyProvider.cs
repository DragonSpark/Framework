using Microsoft.Practices.Unity;
using System.Linq;
using System.Reflection;

namespace Prism.Unity.Windows
{
	public class AssemblyProvider : Modularity.AssemblyProviderBase
	{
		public static AssemblyProvider Instance
		{
			get { return InstanceField; }
		}	static readonly AssemblyProvider InstanceField = new AssemblyProvider();

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