using System;
using System.Linq;
using DragonSpark.Extensions;

namespace DragonSpark.Activation
{
	public class CompositeActivator : IActivator
	{
		readonly IActivator[] activators;

		public CompositeActivator( params IActivator[] activators )
		{
			this.activators = activators;
		}

		public bool CanActivate( Type type, string name = null ) => activators.Any( activator => activator.CanActivate( type, name ) );

		public object Activate( Type type, string name = null ) => activators.Where( activator => activator.CanActivate( type, name ) ).Select( activator => activator.Activate( type, name ) ).NotNull().FirstOrDefault();

		public bool CanConstruct( Type type, params object[] parameters ) => activators.Any( activator => activator.CanConstruct( type, parameters ) );

		public object Construct( Type type, params object[] parameters ) => activators.Where( activator => activator.CanConstruct( type, parameters ) ).Select( activator => activator.Construct( type, parameters ) ).NotNull().FirstOrDefault();
	}
}