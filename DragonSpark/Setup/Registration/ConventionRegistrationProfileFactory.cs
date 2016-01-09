using DragonSpark.Activation.FactoryModel;
using DragonSpark.Extensions;
using PostSharp.Patterns.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DragonSpark.TypeSystem;

namespace DragonSpark.Setup.Registration
{
	public class ConventionRegistrationProfileFactory : FactoryBase<ConventionRegistrationProfile>
	{
		readonly Func<Assembly[]> creator;
		readonly IAttributeProvider provider;

		public ConventionRegistrationProfileFactory( [Required]Func<Assembly[]> creator, [Required]IAttributeProvider provider )
		{
			this.creator = creator;
			this.provider = provider;
		}

		protected virtual Type[] DetermineCandidateTypes( IEnumerable<Assembly> assemblies )
		{
			var result = assemblies.SelectMany( assembly =>
				{
					var types = assembly.DefinedTypes.Where( info => !info.IsAbstract && ( !info.IsNested || info.IsNestedPublic ) )
						.AsTypes()
						.Except( provider.FromMetadata<RegistrationAttribute, IEnumerable<Type>>( assembly, attribute => attribute.IgnoreForRegistration ) );
					return types;
				} ).Prioritize().ToArray();
			return result;
		}

		protected override ConventionRegistrationProfile CreateItem() => creator().Prioritize().ToArray().With( assemblies => new ConventionRegistrationProfile( assemblies, DetermineCandidateTypes( assemblies ) ) );
	}
}