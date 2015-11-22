using DragonSpark.Activation;
using DragonSpark.Runtime;
using Ploeh.AutoFixture;
using System;
using System.Linq;
using System.Reflection;
using DragonSpark.Extensions;
using Microsoft.Practices.ServiceLocation;

namespace DragonSpark.Testing.Framework
{
	/*public class ServiceLocation : ReferenceValue<IServiceLocator>, IServiceLocation
	{
		public bool IsAvailable => Locator != null;

		public IServiceLocator Locator => Item;
	}*/

	class MethodInStackTraceSpecification : SpecificationBase<MethodInfo>
	{
		public MethodInStackTraceSpecification( MethodInfo context ) : base( context )
		{}

		protected override bool IsSatisfiedBy( object context )
		{
			var result = new System.Diagnostics.StackTrace().GetFrames()
				.Select( x => x.GetMethod() )
				.OfType<MethodInfo>()
				.Contains( Context );
			return result;
		}
	}

	class CurrentMethodSpecification : AnySpecification
	{
		public CurrentMethodSpecification( MethodInfo method  ) : base( new MemberInfoSpecification( method ), new MethodInStackTraceSpecification( method ) )
		{}
	}

	public class AmbientLocatorKeyFactory : FactoryBase<MethodInfo, IAmbientKey>
	{
		public static AmbientLocatorKeyFactory Instance { get; } = new AmbientLocatorKeyFactory();

		protected override IAmbientKey CreateFrom( Type resultType, MethodInfo parameter )
		{
			var specification = new CurrentMethodSpecification( parameter );
			var result = new AmbientKey<IServiceLocator>( specification );
			return result;
		}
	}

	public static class ServiceLocatorExtensions
	{
		public static IServiceLocator Prepared( this IServiceLocator @this, MethodBase method )
		{
			var key = method.AsTo<MethodInfo, IAmbientKey>( AmbientLocatorKeyFactory.Instance.Create );
			@this.Register( key );
			return @this;
		}
	}
}