using DragonSpark.ComponentModel;
using DragonSpark.Diagnostics;
using DragonSpark.Extensions;
using DragonSpark.Properties;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using PostSharp.Patterns.Contracts;
using System;
using System.Collections.ObjectModel;
using System.Windows.Markup;

namespace DragonSpark.Setup.Registration
{
	[ContentProperty( nameof(Types) )]
	public class RegisterFrameworkExceptionTypesCommand : SetupCommand
	{
		[Locate, Required]
		public IMessageLogger MessageLogger { [return: Required]get; set; }

		protected override void OnExecute( ISetupParameter parameter )
		{
			MessageLogger.Information(Resources.RegisteringFrameworkExceptionTypes, Priority.Low);
			Types.Each( ExceptionExtensions.RegisterFrameworkExceptionType );
		}

		public Collection<Type> Types { get; } = new Collection<Type>( new [] { typeof(ActivationException), typeof(ResolutionFailedException) } );
	}
}