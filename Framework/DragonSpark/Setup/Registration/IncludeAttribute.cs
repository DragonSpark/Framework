using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DragonSpark.Setup.Registration
{
	[AttributeUsage( AttributeTargets.Assembly )]
	public sealed class IncludeAttribute : Attribute
	{
		public IncludeAttribute( params Type[] typesInAssemblies )
		{
			Assemblies = typesInAssemblies.Select( type => type.GetTypeInfo().Assembly ).ToArray();
		}

		public IEnumerable<Assembly> Assemblies { get; }
	}
}