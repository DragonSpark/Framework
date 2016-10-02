using System;
using DragonSpark.Sources;
using DragonSpark.Sources.Parameterized;
using PostSharp.Aspects;
using PostSharp.Aspects.Dependencies;
using PostSharp.Serialization;

namespace DragonSpark.Aspects
{
	[PSerializable]
	[ProvideAspectRole( "Data" ), LinesOfCodeAvoided( 1 ), AttributeUsage( AttributeTargets.Method )]
	[AspectRoleDependency( AspectDependencyAction.Order, AspectDependencyPosition.After, StandardRoles.Caching )]
	[AspectRoleDependency( AspectDependencyAction.Order, AspectDependencyPosition.After, StandardRoles.Validation )]
	[AspectRoleDependency( AspectDependencyAction.Order, AspectDependencyPosition.Before, StandardRoles.Tracing )]
	public sealed class OriginAttribute : OnMethodBoundaryAspect
	{
		public override void OnSuccess( MethodExecutionArgs args )
		{
			if ( args.ReturnValue != null )
			{
				var creator = args.Instance as ISource;
				if ( creator != null )
				{
					Origin.Default.Set( args.ReturnValue, creator );
				}
			}
		}
	}
}