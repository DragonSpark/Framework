using Microsoft.Practices.ServiceLocation;
using Ploeh.AutoFixture.Kernel;
using Type = DragonSpark.TypeSystem.Type;

namespace DragonSpark.Testing.Framework.Setup.Location
{
	public class AuthorizedServiceLocationRelay : ServiceLocationRelay
	{
		public AuthorizedServiceLocationRelay( IServiceLocator locator, AuthorizedLocationSpecification specification ) : base( locator, specification ) {}
	}

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
			var type = Type.From( request );
			var isSatisfiedBy = specification.IsSatisfiedBy( type );
			var item = isSatisfiedBy ? locator.GetService( type ) : null;
			var result = item ?? new NoSpecimen();
			return result;
		}
	}
}