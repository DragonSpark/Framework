using DragonSpark.Activation;
using DragonSpark.Extensions;
using Microsoft.Practices.ServiceLocation;
using Ploeh.AutoFixture.Kernel;
using System;

namespace DragonSpark.Testing.Framework
{
	public class AuthorizedLocationSpecification : CanLocateSpecification
	{
		readonly IServiceLocationAuthority authorized;

		public AuthorizedLocationSpecification( IServiceLocator locator, IServiceLocationAuthority authorized ) : base( locator )
		{
			this.authorized = authorized;
		}

		protected override bool CanLocate( Type type )
		{
			return authorized.IsAllowed(type) && base.CanLocate( type );
		}
	}

	public class LocateTypeSpecification : CanLocateSpecification
	{
		readonly Type typeToLocate;

		public LocateTypeSpecification( IServiceLocator locator, Type typeToLocate ) : base( locator )
		{
			this.typeToLocate = typeToLocate;
		}

		protected override bool CanLocate( Type type )
		{
			var result = typeToLocate == type && base.CanLocate( type );
			return result;
		}
	}

	public class CanLocateSpecification : IRequestSpecification
	{
		readonly IActivator activator;

		public CanLocateSpecification( IServiceLocator locator ) : this( locator.GetInstance<IActivator>().InvalidIfNull( "Specified ServiceLocator does not have an Activator." ) )
		{}

		public CanLocateSpecification( IActivator activator )
		{
			this.activator = activator;
		}

		public bool IsSatisfiedBy( object request )
		{
			var result = request.AsTo<Type, bool>( CanLocate );
			return result;
		}

		protected virtual bool CanLocate( Type type )
		{
			return activator.CanActivate( type );
		}
	}
}