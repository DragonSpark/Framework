using Microsoft.Practices.ServiceLocation;
using Prism.Logging;
using Prism.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Prism
{
	public class RegisterFrameworkExceptionTypesCommand : SetupCommand
	{
		protected override void Execute( SetupContext context )
		{
			context.Logger.Log(Resources.RegisteringFrameworkExceptionTypes, Category.Debug, Priority.Low);

			var items = DetermineCoreFrameworkTypes().Concat( Types );
			foreach ( var type in items )
			{
				ExceptionExtensions.RegisterFrameworkExceptionType(type);
			}
		}

		protected virtual IEnumerable<Type> DetermineCoreFrameworkTypes()
		{
			yield return typeof(ActivationException);
		}

		public ICollection<Type> Types
		{
			get { return types; }
		}	readonly ICollection<Type> types = new Collection<Type>();
	}
}