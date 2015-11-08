using DragonSpark.Activation;
using DragonSpark.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DragonSpark.Modularity;
using DragonSpark.Runtime;

namespace DragonSpark.Setup
{
	public class ConventionRegistrationProfileProvider : IConventionRegistrationProfileProvider
	{
		readonly IAssemblyProvider locator;

		public ConventionRegistrationProfileProvider( IAssemblyProvider locator )
		{
			this.locator = locator;
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
			var assemblies = locator.GetAssemblies().ToArray();
			var result = assemblies
				.OrderBy( x => x.FromMetadata<RegistrationAttribute, Priority>( z => z.Priority, () => Priority.Normal ) ).ToArray();
			return result;
		}

		protected virtual TypeInfo[] DetermineCandidateTypes( Assembly[] assemblies )
		{
			var result = assemblies
				.SelectMany( assembly => assembly.DefinedTypes.Where( info => !info.IsAbstract ).Except( assembly.FromMetadata<RegistrationAttribute, IEnumerable<TypeInfo>>( attribute => attribute.IgnoreForRegistration.AsTypeInfos() ) ) ).ToArray();
			return result;
		}
	}
}