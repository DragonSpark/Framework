using DragonSpark.Extensions;
using DragonSpark.Sources.Parameterized.Caching;
using DragonSpark.TypeSystem;
using PostSharp.Aspects;
using PostSharp.Aspects.Configuration;
using PostSharp.Aspects.Dependencies;
using PostSharp.Aspects.Serialization;
using System;
using System.Collections.Generic;

namespace DragonSpark.Aspects
{
	[OnMethodBoundaryAspectConfiguration( SerializerType = typeof(MsilAspectSerializer) )]
	[LinesOfCodeAvoided( 3 ), ProvideAspectRole( StandardRoles.Validation ), AspectRoleDependency( AspectDependencyAction.Order, AspectDependencyPosition.Before, StandardRoles.Caching )]
	public sealed class RecursionGuardAttribute : OnMethodBoundaryAspect
	{
		readonly int maxCallCount;
		readonly ICache<IDictionary<int, int>> cache;

		public RecursionGuardAttribute( int maxCallCount = 2 ) : this( new DecoratedSourceCache<IDictionary<int, int>>( new ThreadLocalSourceCache<IDictionary<int, int>>( () => new Dictionary<int, int>() ) ), maxCallCount ) {}

		RecursionGuardAttribute( ICache<IDictionary<int, int>> cache, int maxCallCount = 2 )
		{
			this.maxCallCount = maxCallCount;
			this.cache = cache;
		}

		public override void OnEntry( MethodExecutionArgs args )
		{
			var current = Current( args, 1 );
			if ( current >= maxCallCount )
			{
				throw new InvalidOperationException( $"Recursion detected in method {new MethodFormatter(args.Method).ToString()}" );
			}

			base.OnEntry( args );
		}

		int Current( MethodExecutionArgs args, int move )
		{
			var dictionary = cache.Get( args.Instance ?? args.Method.DeclaringType );
			var key = Keys.For( args );
			var result = dictionary[key] = dictionary.Ensure( key, i => 0 ) + move;
			return result;
		}

		public override void OnExit( MethodExecutionArgs args )
		{
			base.OnExit( args );
			Current( args, -1 );
		}
	}
}