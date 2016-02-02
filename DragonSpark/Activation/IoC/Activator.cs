using DragonSpark.Extensions;
using Microsoft.Practices.Unity;
using PostSharp.Patterns.Contracts;
using System;
using System.Linq;
using DragonSpark.Setup.Registration;

namespace DragonSpark.Activation.IoC
{
	class Activator : IActivator
	{
		readonly IUnityContainer container;
		readonly IResolutionSupport support;
		readonly RegisterAllClassesCommand command;

		public Activator( [Required]IUnityContainer container, [Required]IResolutionSupport support, [Required]RegisterAllClassesCommand command )
		{
			this.container = container;
			this.support = support;
			this.command = command;
		}

		public bool CanActivate( Type type, string name = null ) => support.CanResolve( type, name );

		public object Activate( Type type, string name = null ) => container.TryResolve( type, name );

		public bool CanConstruct( Type type, params object[] parameters ) => support.CanResolve( type, null, parameters );

		public object Construct( Type type, params object[] parameters )
		{
			using ( var child = container.CreateChildContainer() )
			{
				parameters.NotNull().Select( o => new InstanceRegistrationParameter( o ) ).Each( command.ExecuteWith );

				var result = child.TryResolve( type );
				return result;
			}
		}
	}
}