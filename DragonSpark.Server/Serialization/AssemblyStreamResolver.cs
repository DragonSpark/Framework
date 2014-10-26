using System;
using System.IO;
using System.Reflection;

namespace DragonSpark.Serialization
{
	public class AssemblyStreamResolver : IStreamResolver
	{
		readonly Assembly assembly;
		readonly string resourceName;

		public AssemblyStreamResolver( string assemblyName, string resourceName ) : this( Assembly.Load( assemblyName ), resourceName )
		{}

		public AssemblyStreamResolver( Assembly assembly, string resourceName )
		{
			this.assembly = assembly;
			this.resourceName = resourceName;
		}

		public Assembly Assembly
		{
			get { return assembly; }
		}

		public string ResourceName
		{
			get { return resourceName; }
		}

		public Stream ResolveStream()
		{
			var result = assembly.GetManifestResourceStream( ResourceName );
			if ( result != null )
			{
				return result;
			}
			throw new InvalidOperationException( string.Format( "Could not find resource {0} in assembly {1}.", ResourceName, assembly.FullName ) );
		}
	}
}