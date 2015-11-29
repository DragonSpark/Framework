using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using DragonSpark.Extensions;
using DragonSpark.Properties;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;

namespace DragonSpark.Setup
{
	public class RegisterFrameworkExceptionTypesCommand : SetupCommand
	{
		protected override void Execute( SetupContext context )
		{
			context.Logger.Information(Resources.RegisteringFrameworkExceptionTypes, Priority.Low);

			var items = DetermineCoreFrameworkTypes().Concat( Types );
			foreach ( var type in items )
			{
				ExceptionExtensions.RegisterFrameworkExceptionType(type);
			}
		}

		protected virtual IEnumerable<Type> DetermineCoreFrameworkTypes()
		{
			yield return typeof(ActivationException);
			yield return typeof(ResolutionFailedException);
		}

		public ICollection<Type> Types { get; } = new Collection<Type>();
	}
}