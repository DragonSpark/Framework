using DragonSpark.Extensions;
using DragonSpark.TypeSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DragonSpark.Setup.Registration
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
			
			var candidates = DetermineCandidateTypes( assemblies );

			var result = new ConventionRegistrationProfile( assemblies, candidates );
			return result;
		}

		protected virtual Assembly[] DetermineAssemblies()
		{
			var result = provider.GetAssemblies().Prioritize().ToArray();
			return result;
		}

		protected virtual Type[] DetermineCandidateTypes( Assembly[] assemblies )
		{
			var result = assemblies.SelectMany( assembly =>
				{
					var types = assembly.DefinedTypes.Where( info => !info.IsAbstract && ( !info.IsNested || info.IsNestedPublic ) )
						.AsTypes()
						.Except( assembly.FromMetadata<RegistrationAttribute, IEnumerable<Type>>( attribute => attribute.IgnoreForRegistration ) );
					return types;
				} ).Prioritize().ToArray();
			return result;
		}
	}
}