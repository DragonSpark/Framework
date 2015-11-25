using Microsoft.Practices.ServiceLocation;
using Ploeh.AutoFixture;
using System;
using System.Collections.Generic;

namespace DragonSpark.Testing.Framework
{
	public interface IServiceLocationAuthority
	{
		void Register( Type item, bool enabled );

		bool IsAllowed( Type type );
	}

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