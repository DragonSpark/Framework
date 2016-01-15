using System;
using System.Reflection;
using AutoMapper.Internal;
using DragonSpark.Extensions;
using Microsoft.Practices.ServiceLocation;
using Ploeh.AutoFixture.Kernel;

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
			var type = request.AsTo<ParameterInfo, Type>( info => info.ParameterType ) ?? request.AsTo<MemberInfo, Type>( info => info.GetMemberType() ) ?? request as Type;
			var item = specification.IsSatisfiedBy( type ) ? locator.GetService( type ) : null;
			var result = item ?? new NoSpecimen();
			return result;
		}
	}
}