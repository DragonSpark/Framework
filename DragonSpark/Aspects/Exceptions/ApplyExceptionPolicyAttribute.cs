using DragonSpark.Extensions;
using DragonSpark.Sources;
using DragonSpark.Sources.Parameterized;
using Polly;
using PostSharp.Aspects;
using PostSharp.Aspects.Advices;
using PostSharp.Aspects.Dependencies;
using System;
using Activator = DragonSpark.Activation.Activator;

namespace DragonSpark.Aspects.Exceptions
{
	[IntroduceInterface( typeof(IPolicySource) )]
	[ProvideAspectRole( StandardRoles.ExceptionHandling ), LinesOfCodeAvoided( 1 ), 
		AspectRoleDependency( AspectDependencyAction.Order, AspectDependencyPosition.After, KnownRoles.ParameterValidation ),
		AspectRoleDependency( AspectDependencyAction.Order, AspectDependencyPosition.After, KnownRoles.EnhancedValidation ),
		AspectRoleDependency( AspectDependencyAction.Order, AspectDependencyPosition.After, StandardRoles.Validation )
		]
	public sealed class ApplyExceptionPolicyAttribute : ApplyInstanceAspectBase, IPolicySource
	{
		readonly static Func<Type, Policy> Source = Activator.Default.Get<Policy>;

		readonly ISource<Policy> source;

		public ApplyExceptionPolicyAttribute( Type policyType ) : this( policyType, Source ) {}

		ApplyExceptionPolicyAttribute( Type policyType, Func<Type, Policy> source ) : this( source.Fixed( policyType ) ) {}

		ApplyExceptionPolicyAttribute( ISource<Policy> source ) : base( Support.Default )
		{
			this.source = source;
		}

		public Policy Get() => source.Get();
		object ISource.Get() => Get();
	}
}