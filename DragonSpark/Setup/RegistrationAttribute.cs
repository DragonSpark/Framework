using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DragonSpark.Setup
{
	[AttributeUsage( AttributeTargets.Assembly )]
	public sealed class RegistrationAttribute : Attribute
	{
		readonly Priority priority;
		readonly Type[] ignoreForRegistration;

		public RegistrationAttribute( Priority priority, params Type[] ignoreForRegistration )
		{
			this.priority = priority;
			this.ignoreForRegistration = ignoreForRegistration;
		}

		public Priority Priority
		{
			get { return priority; }
		}

		public IEnumerable<Type> IgnoreForRegistration
		{
			get { return ignoreForRegistration; }
		}
	}

	[AttributeUsage( AttributeTargets.Assembly )]
	public sealed class IncludeAttribute : Attribute
	{
		readonly Assembly[] assemblies;

		public IncludeAttribute( params Type[] typesInAssemblies )
		{
			assemblies = typesInAssemblies.Select( type => type.GetTypeInfo().Assembly ).ToArray();
		}

		public IEnumerable<Assembly> Assemblies
		{
			get { return assemblies; }
		}
	}
}