using PostSharp.Aspects;
using System;
using System.Reflection;

namespace DragonSpark.Aspects.Build
{
	public class MethodBasedAspectInstanceLocator<T> : IAspectInstanceLocator where T : IAspect
	{
		readonly static Func<MethodInfo, AspectInstance> Factory = AspectInstanceFactory<T, MethodInfo>.Default.Get;

		readonly Func<Type, MethodInfo> methodSource;
		readonly Func<MethodInfo, AspectInstance> inner;

		public MethodBasedAspectInstanceLocator( IMethodStore store ) : this( store.Get, Factory ) {}

		public MethodBasedAspectInstanceLocator( Func<Type, MethodInfo> methodSource, Func<MethodInfo, AspectInstance> inner )
		{
			this.methodSource = methodSource;
			this.inner = inner;
		}

		public AspectInstance Get( Type parameter )
		{
			var method = methodSource( parameter );
			var result = method != null ? inner( method ) : null;
			return result;
		}
	}
}