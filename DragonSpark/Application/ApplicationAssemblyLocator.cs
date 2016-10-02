using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DragonSpark.Sources.Parameterized;

namespace DragonSpark.Application
{
	public class ApplicationAssemblyLocator : ParameterizedSourceBase<IEnumerable<Assembly>, Assembly>
	{
		public static ApplicationAssemblyLocator Default { get; } = new ApplicationAssemblyLocator();
		ApplicationAssemblyLocator() {}

		public override Assembly Get( IEnumerable<Assembly> parameter ) => parameter.SingleOrDefault( assembly => assembly.IsDefined( typeof(ApplicationAttribute) ) );
	}
}