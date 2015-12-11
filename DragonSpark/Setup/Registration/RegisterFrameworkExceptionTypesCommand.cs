using DragonSpark.Extensions;
using DragonSpark.Properties;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DragonSpark.Setup.Registration
{
	public class RegisterFrameworkExceptionTypesCommand : SetupCommand
	{
		protected override void Execute( SetupContext context )
		{
			context.Logger.Information(Resources.RegisteringFrameworkExceptionTypes, Priority.Low);
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