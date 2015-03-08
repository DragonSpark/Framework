using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Reflection;
using DragonSpark.Extensions;

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
			Contract.Requires( assembly != null );
			Contract.Requires( !string.IsNullOrEmpty( resourceName ) );
			this.assembly = assembly;
			this.resourceName = resourceName;
		}

		/*[ContractInvariantMethod]
		void Invariant()
		{
			Contract.Invariant( assembly != null );
			Contract.Invariant( !string.IsNullOrEmpty( resourceName ) );
		}*/

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