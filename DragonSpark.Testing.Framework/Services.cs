using DragonSpark.Activation;
using DragonSpark.Runtime;
using Ploeh.AutoFixture;
using System;
using System.Linq;
using System.Reflection;
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
}