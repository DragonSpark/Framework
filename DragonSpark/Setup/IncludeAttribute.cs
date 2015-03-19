using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DragonSpark.Setup
{
	[AttributeUsage( AttributeTargets.Assembly )]
	public sealed class IncludeAttribute : Attribute
	{
		readonly Assembly[] assemblies;

		public IncludeAttribute( params Type[] typesInAssemblies )
		{
			assemblies = typesInAssemblies.Select( type => IntrospectionExtensions.GetTypeInfo( type ).Assembly ).ToArray();
		}

		public IEnumerable<Assembly> Assemblies
		{
			get { return assemblies; }
		}
	}
}