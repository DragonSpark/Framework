using DragonSpark.Sources.Parameterized;
using DragonSpark.Sources.Parameterized.Caching;
using PostSharp.Aspects;
using PostSharp.Aspects.Configuration;
using PostSharp.Aspects.Dependencies;
using PostSharp.Aspects.Serialization;
using System;
using System.Reflection;

namespace DragonSpark.Aspects
{
	[MethodInterceptionAspectConfiguration( SerializerType = typeof(MsilAspectSerializer) ), AttributeUsage( AttributeTargets.Method | AttributeTargets.Property )]
	[LinesOfCodeAvoided( 6 ), 
		ProvideAspectRole( StandardRoles.Caching ), 
		AspectRoleDependency( AspectDependencyAction.Order, AspectDependencyPosition.Before, StandardRoles.Validation ),
		AspectRoleDependency( AspectDependencyAction.Order, AspectDependencyPosition.After, StandardRoles.Threading )
		]
	public class FreezeAttribute : MethodInterceptionAspect, IInstanceScopedAspect
	{
		readonly Func<object, IAspectHub> hubSource;
		readonly static Func<object, IAspectHub> HubSource = AspectHub.Default.ToDelegate();

		public FreezeAttribute() : this( HubSource ) {}

		protected FreezeAttribute( Func<object, IAspectHub> hubSource )
		{
			this.hubSource = hubSource;
		}

		public override void RuntimeInitialize( MethodBase method ) => Profile = new MethodProfile( (MethodInfo)method );

		MethodProfile Profile { get; set; }

		public object CreateInstance( AdviceArgs adviceArgs )
		{
			var result = Profile.Create( Profile.Method );

			hubSource( adviceArgs.Instance )?.Register( result );

			return result;
		}

		void IInstanceScopedAspect.RuntimeInitializeInstance() {}

		sealed class SingleParameterFreeze : InstanceFreezeBase
		{
			readonly IArgumentCache<object, object> cache;

			public SingleParameterFreeze( MethodInfo method ) : this( new ArgumentCache<object, object>(), method ) {}

			SingleParameterFreeze( IArgumentCache<object, object> cache, MethodInfo method ) : base( new CacheParameterHandler<object, object>( cache ), method  )
			{
				this.cache = cache;
			}

			public override void OnInvoke( MethodInterceptionArgs args ) => args.ReturnValue = cache.GetOrSet( args.Arguments[0], args.GetReturnValue );
		}

		struct MethodProfile
		{
			public MethodProfile( MethodInfo method ) : this( method, method.GetParameters().Length == 1 ? new Func<MethodInfo, FreezeAttribute>( info => new SingleParameterFreeze( info ) ) : ( info => new Freeze( info ) ) ) {}

			MethodProfile( MethodInfo method, Func<MethodInfo, FreezeAttribute> create )
			{
				Method = method;
				Create = create;
			}

			public MethodInfo Method { get; }
			public Func<MethodInfo, FreezeAttribute> Create { get; }
		}

		abstract class InstanceFreezeBase : FreezeAttribute, IParameterAwareHandler, IMethodAware
		{
			readonly IParameterAwareHandler handler;
			protected InstanceFreezeBase( IParameterAwareHandler handler, MethodInfo method )
			{
				this.handler = handler;
				Method = method;
			}

			public bool Handles( object parameter ) => handler.Handles( parameter );

			public bool Handle( object parameter, out object handled ) => handler.Handle( parameter, out handled );

			public MethodInfo Method { get; }
		}

		sealed class Freeze : InstanceFreezeBase
		{
			readonly IArgumentCache<object[], object> cache;

			public Freeze( MethodInfo method ) : this( new ArgumentCache<object[], object>(), method ) {}

			Freeze( IArgumentCache<object[], object> cache, MethodInfo method ) : base( new CacheParameterHandler<object[], object>( cache ), method  )
			{
				this.cache = cache;
			}

			public override void OnInvoke( MethodInterceptionArgs args ) => args.ReturnValue = cache.GetOrSet( args.Arguments.ToArray(), args.GetReturnValue );
		}
	}
}