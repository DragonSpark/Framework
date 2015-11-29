using System;
using DragonSpark.Activation;
using DragonSpark.Extensions;
using Microsoft.Practices.ServiceLocation;
using Ploeh.AutoFixture.Kernel;

namespace DragonSpark.Testing.Framework.Setup
{
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