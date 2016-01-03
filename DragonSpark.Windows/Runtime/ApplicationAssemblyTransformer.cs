using DragonSpark.Extensions;
using DragonSpark.Setup.Registration;
using System.Collections.Generic;
using System.Reflection;

namespace DragonSpark.Windows.Runtime
{
	[RegisterFactoryForResult]
	public class ApplicationAssemblyTransformer : TypeSystem.ApplicationAssemblyTransformer
	{
		public new static ApplicationAssemblyTransformer Instance { get; } = new ApplicationAssemblyTransformer();

		public ApplicationAssemblyTransformer() : base( DetermineCoreAssemblies() )
		{}

		static IEnumerable<Assembly> DetermineCoreAssemblies()
		{
			var assembly = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();
			var result = assembly.Append( MethodBase.GetCurrentMethod().DeclaringType.Assembly );
			return result;
		}
	}
}