using DragonSpark.TypeSystem;
using PostSharp.Aspects;
using PostSharp.Aspects.Configuration;
using PostSharp.Aspects.Dependencies;
using PostSharp.Aspects.Serialization;
using System;
using System.Threading;

namespace DragonSpark.Aspects
{
	[OnMethodBoundaryAspectConfiguration( SerializerType = typeof(MsilAspectSerializer) )]
	[LinesOfCodeAvoided( 3 ), ProvideAspectRole( StandardRoles.Validation ), AspectRoleDependency( AspectDependencyAction.Order, AspectDependencyPosition.Before, StandardRoles.Caching )]
	public class RecursionGuardAttribute : OnMethodBoundaryAspect, IInstanceScopedAspect
	{
		readonly int maxCallCount;

		public RecursionGuardAttribute( int maxCallCount = 2 )
		{
			this.maxCallCount = maxCallCount;
		}
		
		sealed class RecursionGuard : RecursionGuardAttribute
		{
			readonly ThreadLocal<int> count = new ThreadLocal<int>();

			public RecursionGuard( int maxCallCount ) : base( maxCallCount ) {}

			public override void OnEntry( MethodExecutionArgs args )
			{
				var current = Current( 1 );
				if ( current >= maxCallCount )
				{
					throw new InvalidOperationException( $"Recursion detected in method {new MethodFormatter( args.Method ).ToString()}" );
				}

				base.OnEntry( args );
			}

			int Current( int move )
			{
				count.Value = count.Value + move;
				return count.Value;
			}

			public override void OnExit( MethodExecutionArgs args )
			{
				base.OnExit( args );
				Current( -1 );
			}
		}

		public object CreateInstance( AdviceArgs adviceArgs ) => new RecursionGuard( maxCallCount );
		void IInstanceScopedAspect.RuntimeInitializeInstance() {}
	}
}