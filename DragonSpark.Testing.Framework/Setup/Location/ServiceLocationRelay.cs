using System;
using DragonSpark.Extensions;
using Microsoft.Practices.ServiceLocation;
using Ploeh.AutoFixture.Kernel;

namespace DragonSpark.Testing.Framework.Setup.Location
{
	public class ServiceLocationRelay : ISpecimenBuilder
	{
		readonly IServiceLocator locator;
		readonly IRequestSpecification specification;

		public ServiceLocationRelay( IServiceLocator locator ) : this( locator, new CanLocateSpecification( locator ) )
		{}

		public ServiceLocationRelay( IServiceLocator locator, IRequestSpecification specification )
		{
			this.locator = locator;
			this.specification = specification;
		}

		public object Create( object request, ISpecimenContext context )
		{
			var item = specification.IsSatisfiedBy( request ) ? request.AsTo<Type, object>( locator.GetService ) : null;
			var result = item ?? new NoSpecimen();
			return result;
		}
	}
}