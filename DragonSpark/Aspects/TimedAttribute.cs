using DragonSpark.Sources;
using DragonSpark.Sources.Parameterized;
using PostSharp.Aspects;
using PostSharp.Aspects.Configuration;
using PostSharp.Aspects.Dependencies;
using PostSharp.Aspects.Serialization;
using System;
using System.Reflection;

namespace DragonSpark.Aspects
{
	[ProvideAspectRole( StandardRoles.Tracing ), LinesOfCodeAvoided( 4 )]
	[
		AspectRoleDependency( AspectDependencyAction.Order, AspectDependencyPosition.After, StandardRoles.Caching ), 
		AspectRoleDependency( AspectDependencyAction.Order, AspectDependencyPosition.Before, StandardRoles.Validation )
	]
	[MethodInterceptionAspectConfiguration( SerializerType = typeof(MsilAspectSerializer) )]
	public sealed class TimedAttribute : MethodInterceptionAspect
	{
		readonly Func<Func<MethodBase, IDisposable>> source;

		public TimedAttribute() : this( "Executed Method '{@Method}'" ) {}

		public TimedAttribute( string template ) : this( Diagnostics.Configuration.TimedOperationFactory.Fixed( template ).ToDelegate() ) {}

		TimedAttribute( Func<Func<MethodBase, IDisposable>> source )
		{
			this.source = source;
		}

		public override void OnInvoke( MethodInterceptionArgs args )
		{
			using ( source()( args.Method ) )
			{
				base.OnInvoke( args );
			}
		}
	}
}