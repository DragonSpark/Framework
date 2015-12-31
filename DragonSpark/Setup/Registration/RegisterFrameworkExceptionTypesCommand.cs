using DragonSpark.Extensions;
using DragonSpark.Properties;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using DragonSpark.ComponentModel;
using DragonSpark.Diagnostics;
using PostSharp.Patterns.Contracts;

namespace DragonSpark.Setup.Registration
{
	public class RegisterFrameworkExceptionTypesCommand : SetupCommand
	{
		[Activate, Required]
		public IMessageLogger MessageLogger { [return: Required]get; set; }

		protected override void OnExecute( ISetupParameter parameter )
		{
			MessageLogger.Information(Resources.RegisteringFrameworkExceptionTypes, Priority.Low);
			DetermineCoreFrameworkTypes().Concat( Types ).Each( ExceptionExtensions.RegisterFrameworkExceptionType );
		}

		protected virtual IEnumerable<Type> DetermineCoreFrameworkTypes()
		{
			yield return typeof(ActivationException);
			yield return typeof(ResolutionFailedException);
		}

		public Collection<Type> Types { get; } = new Collection<Type>();
	}
}