using DragonSpark.Extensions;
using DragonSpark.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DragonSpark.Setup
{
	public class ConventionRegistrationProfileProvider : IConventionRegistrationProfileProvider
	{
		readonly IAssemblyProvider provider;

		public ConventionRegistrationProfileProvider( IAssemblyProvider provider )
		{
			this.provider = provider;
		}

		public ConventionRegistrationProfile Retrieve()
		{
			var assemblies = DetermineAssemblies();
			
			var types = DetermineCandidateTypes( assemblies );

			var result = new ConventionRegistrationProfile( assemblies, assemblies.SelectMany( assembly => assembly.FromMetadata<IncludeAttribute, IEnumerable<Assembly>>( attribute => attribute.Assemblies ) ).Except( assemblies ).ToArray(), types );
			return result;
		}

		protected virtual Assembly[] DetermineAssemblies()
		{
			var assemblies = provider.GetAssemblies().ToArray();
			var result = assemblies
				.OrderBy( x => x.FromMetadata<RegistrationAttribute, Priority>( z => z.Priority, () => Priority.Normal ) ).ToArray();
			return result;
		}

		protected virtual Type[] DetermineCandidateTypes( Assembly[] assemblies )
		{
			var result = assemblies
				.SelectMany( assembly => assembly.DefinedTypes.Where( info => !info.IsAbstract )
				.AsTypes()
				.Except( assembly.FromMetadata<RegistrationAttribute, IEnumerable<Type>>( attribute => attribute.IgnoreForRegistration ) ) )
				.ToArray();
			return result;
		}
	}
}