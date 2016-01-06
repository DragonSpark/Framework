using DragonSpark.TypeSystem;
using Microsoft.Practices.Unity;
using System.Linq;
using System.Reflection;

namespace DragonSpark.Windows.Runtime
{
	public class FileSystemAssemblySource : AssemblySourceBase
	{
		public static FileSystemAssemblySource Instance { get; } = new FileSystemAssemblySource();

		protected override Assembly[] CreateItem()
		{
			var result = AllClasses.FromAssembliesInBasePath( includeUnityAssemblies: true )
				.Where( x => x.Namespace != null )
				.GroupBy( type => type.Assembly )
				.Select( types => types.Key ).ToArray();
			return result;
		}
	}
}