using DragonSpark.Extensions;
using System.Reflection;

namespace DragonSpark.Windows.Runtime
{
	public class ApplicationAssemblyTransformer : TypeSystem.ApplicationAssemblyTransformer
	{
		public static ApplicationAssemblyTransformer Instance { get; } = new ApplicationAssemblyTransformer();

		public ApplicationAssemblyTransformer() : base( DetermineCoreAssemblies() ) {}

		static Assembly[] DetermineCoreAssemblies()
		{
			var assembly = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();
			var result = assembly.Append( MethodBase.GetCurrentMethod().DeclaringType.Assembly ).Fixed();
			return result;
		}
	}
}