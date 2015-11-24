using DragonSpark.Activation;
using DragonSpark.Extensions;
using Microsoft.Practices.ServiceLocation;
using Ploeh.AutoFixture.Kernel;
using System;
using System.Linq;

namespace DragonSpark.Testing.Framework
{
	public class CanLocateSpecification : IRequestSpecification
	{
		readonly IServiceLocator locator;
		readonly Type[] passThrough;

		/*public CanLocateSpecification( IServiceLocator locator ) : this( locator, FixtureContext.GetCurrent().GetCurrentMethod().GetParameters().Where( info => info.IsDefined( typeof(SkipLocationAttribute) ) ).Select( info => info.ParameterType ).ToArray() )
		{}*/

		public CanLocateSpecification( IServiceLocator locator, params Type[] passThrough )
		{
			this.locator = locator;
			this.passThrough = passThrough;
		}

		public bool IsSatisfiedBy( object request )
		{
			var result = request.AsTo<Type, bool>( type => !passThrough.Contains( type ) && locator.GetInstance<IActivator>().Transform( activator => activator.CanActivate( type ) ) );
			return result;
		}
	}
}