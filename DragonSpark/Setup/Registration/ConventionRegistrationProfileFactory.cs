using DragonSpark.Activation.FactoryModel;
using DragonSpark.Extensions;
using DragonSpark.TypeSystem;
using PostSharp.Patterns.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Type = System.Type;

namespace DragonSpark.Setup.Registration
{
	[Discoverable]
	public class ConventionRegistrationProfileFactory : FactoryBase<ConventionRegistrationProfile>
	{
		readonly Assembly[] assemblies;

		public ConventionRegistrationProfileFactory( [Required]Assembly[] assemblies )
		{
			this.assemblies = assemblies;
		}

		protected virtual Type[] DetermineCandidateTypes()
		{
			var result = assemblies.SelectMany( assembly =>
				{
					var types = assembly.DefinedTypes.Where( info => !info.IsAbstract && ( !info.IsNested || info.IsNestedPublic ) )
						.AsTypes()
						.Except( assembly.From<RegistrationAttribute, IEnumerable<Type>>( attribute => attribute.IgnoreForRegistration ) );
					return types;
				} ).Prioritize().ToArray();
			return result;
		}

		protected override ConventionRegistrationProfile CreateItem() => new ConventionRegistrationProfile( DetermineCandidateTypes() );
	}
}