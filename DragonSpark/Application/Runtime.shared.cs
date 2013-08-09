using System.Diagnostics;
using DragonSpark.Extensions;
using Microsoft.Practices.ServiceLocation;

namespace DragonSpark.Application
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1724:TypeNamesShouldNotMatchNamespaces", Justification = "Denied." )]
    public static class Runtime
	{
		public static bool IsRunningUnderDebugOrLocalhost()
		{
			var result = Debugger.IsAttached || ServiceLocator.Current.TryGetInstance<IApplicationContext>().Transform( x => x.Location.Transform( y => y.Host ).Transform( y => y.Contains( "::1" ) || y.Contains( "localhost" ) || y.Contains( "127.0.0.1" ) ) );
			return result;
		}
	}
}