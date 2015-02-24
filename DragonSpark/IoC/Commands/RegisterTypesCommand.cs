using System;
using System.Collections.Generic;
using DragonSpark.Application.Runtime;
using DragonSpark.ComponentModel;
using DragonSpark.Extensions;
using Microsoft.Practices.Unity;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows.Markup;
using DragonSpark.Activation.IoC.Commands;

namespace DragonSpark.Application.IoC.Commands
{

	[ContentProperty( "IgnoreNamespacesFromTypes" )]
	public class ApplyRegistrationsCommand : Activation.IoC.Commands.ApplyRegistrationsCommand
	{
		[Activate( typeof(AssemblyLocator) )]
		public override IAssemblyLocator Locator
		{
			get { return base.Locator; }
			set { base.Locator = value; }
		}

		protected override void RegisterBasedOnConvention( RegistrationContext context )
		{
			var ignore = context.Application.SelectMany( assembly => assembly.FromMetadata<RegistrationAttribute, IEnumerable<Type>>( attribute => attribute.IgnoreForRegistration ) ).ToArray();
			var register = context.Candidates.Where( x => WithMappings.FromMatchingInterface( x )
				.Any( found => found.IsPublic && /*context.Application.Contains( found.Assembly ) &&*/ !ignore.Contains( found ) )
				).ToArray();
			register.Apply( type => Trace.WriteLine( string.Format( "Registering for convention: {0}", type.FullName ) ) );
			context.Container.RegisterTypes( register, WithMappings.FromMatchingInterface, WithName.Default, DetermineLifetimeContainer<ContainerControlledLifetimeManager>, overwriteExistingMappings: true );
		}

		/*protected override IEnumerable<Type> DetermineCandidateTypes()
		{
			var result = AllClasses.FromAssembliesInBasePath().Where( x => x.Namespace != null );
			return result;
		}*/
	}
}