using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DragonSpark.Extensions;

namespace DragonSpark.Setup
{
	public class ConventionRegistrationProfileProvider : IConventionRegistrationProfileProvider
	{
		readonly IAssemblyLocator locator;

		public ConventionRegistrationProfileProvider( IAssemblyLocator locator )
		{
			this.locator = locator;
		}

		public ConventionRegistrationProfile Retrieve()
		{
			var assemblies = DetermineAssemblies();
			
			var types = DetermineCandidateTypes( assemblies );

			var result = new ConventionRegistrationProfile( assemblies, assemblies.SelectMany<Assembly, Assembly>( assembly => assembly.FromMetadata<IncludeAttribute, IEnumerable<Assembly>>( attribute => attribute.Assemblies ) ).ToArray(), types );
			return result;
		}

		protected virtual Assembly[] DetermineAssemblies()
		{
			var assemblies = locator.GetApplicationAssemblies().ToArray();
			var result = assemblies
				.OrderBy( x => x.FromMetadata<RegistrationAttribute, Priority>( z => z.Priority, () => Priority.Normal ) ).ToArray();
			return result;
		}

		protected virtual TypeInfo[] DetermineCandidateTypes( Assembly[] assemblies )
		{
			var result = assemblies
				.SelectMany( assembly => assembly.DefinedTypes.Except( assembly.FromMetadata<RegistrationAttribute, IEnumerable<TypeInfo>>( attribute => attribute.IgnoreForRegistration.AsTypeInfos() ) ) ).ToArray();
			return result;
		}
	}
}