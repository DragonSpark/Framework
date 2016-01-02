using System;
using System.Collections.Generic;
using DragonSpark.Testing.Framework.Extensions;
using Microsoft.Practices.ServiceLocation;
using Ploeh.AutoFixture;

namespace DragonSpark.Testing.Framework.Setup.Location
{
	class ServiceLocationAuthority : IServiceLocationAuthority
	{
		readonly IFixture fixture;
		readonly IServiceLocator locator;
		readonly ICollection<Type> blacklist = new List<Type>();

		public ServiceLocationAuthority( IFixture fixture ) : this( fixture, fixture.GetLocator() )
		{}

		public ServiceLocationAuthority( IFixture fixture, IServiceLocator locator )
		{
			this.fixture = fixture;
			this.locator = locator;
		}

		public void Register( Type item, bool enabled )
		{
			if ( enabled )
			{
				fixture.Customizations.Insert( 0, new ServiceLocationRelay( locator, new LocateTypeSpecification( locator, item ) ) );
			}
			else
			{
				blacklist.Add( item );
			}
		}

		public bool IsAllowed( Type type )
		{
			var result = !blacklist.Contains( type );
			return result;
		}
	}
}