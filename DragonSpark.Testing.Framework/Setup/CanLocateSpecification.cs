using System;
using System.ComponentModel.DataAnnotations;
using DragonSpark.Activation;
using DragonSpark.Extensions;
using DragonSpark.TypeSystem;
using Microsoft.Practices.ServiceLocation;
using Ploeh.AutoFixture.Kernel;
using Type = DragonSpark.TypeSystem.Type;

namespace DragonSpark.Testing.Framework.Setup
{
	public class CanLocateSpecification : IRequestSpecification
	{
		readonly IActivator activator;

		public CanLocateSpecification( IServiceLocator locator ) : this( locator.GetInstance<IActivator>() )
		{}

		public CanLocateSpecification( [Required]IActivator activator )
		{
			this.activator = activator;
		}

		public bool IsSatisfiedBy( object request ) => Type.From( request ).With( CanLocate );

		protected virtual bool CanLocate( System.Type type ) => activator.CanActivate( type );
	}
}