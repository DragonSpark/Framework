using DragonSpark.Extensions;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DragonSpark.Application.Runtime
{
	public class AssemblyProvider : Activation.AssemblyProvider
	{
		public AssemblyProvider( Func<Assembly, bool> filter = null ) : base( filter )
		{}

		protected override Assembly[] DetermineAllAssemblies()
		{
			var result = AllClasses.FromAssembliesInBasePath( includeUnityAssemblies: true ).Where( x => x.Namespace != null ).Select( type => type.Assembly ).Distinct().ToArray();
			return result;
		}

		protected override IEnumerable<Assembly> DetermineCoreAssemblies()
		{
			var result = base.DetermineCoreAssemblies().Append( Assembly.GetExecutingAssembly() );
			return result;
		}
	}
}