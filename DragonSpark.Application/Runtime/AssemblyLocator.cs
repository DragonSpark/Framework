using DragonSpark.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DragonSpark.Setup;
using Microsoft.Practices.Unity;

namespace DragonSpark.Application.Runtime
{
	public class AssemblyLocator : IAssemblyLocator
	{
		public IEnumerable<Assembly> GetApplicationAssemblies()
		{
			var namespaces = new[] { typeof(IAssemblyLocator).Assembly, Assembly.GetExecutingAssembly() }.Select( assembly => assembly.GetRootNamespace() ).Distinct().ToArray();
			var result = GetAllAssemblies().Where( assembly => assembly.IsDefined( typeof(RegistrationAttribute) ) || namespaces.Any( s => assembly.GetName().Name.StartsWith( s ) ) ).ToArray();
			return result;
		}

		public IEnumerable<Assembly> GetAllAssemblies()
		{
			var result = AllClasses.FromAssembliesInBasePath( includeUnityAssemblies: true ).Where( x => x.Namespace != null ).Select( type => type.Assembly ).Distinct().ToArray();
			return result;
		}
	}
}