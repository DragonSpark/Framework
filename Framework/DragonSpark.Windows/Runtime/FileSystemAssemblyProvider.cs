using DragonSpark.TypeSystem;
using Microsoft.Practices.Unity;
using System.Linq;
using System.Reflection;

namespace DragonSpark.Windows.Runtime
{
	public class FileSystemAssemblyProvider : AssemblyProviderBase
	{
		public static FileSystemAssemblyProvider Instance { get; } = new FileSystemAssemblyProvider();

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