using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Unity;

namespace Prism.Unity
{
	public class RegisterFrameworkExceptionTypesCommand : Prism.RegisterFrameworkExceptionTypesCommand
	{
		protected override IEnumerable<Type> DetermineCoreFrameworkTypes()
		{
			return base.DetermineCoreFrameworkTypes().Concat( new[] { typeof(ResolutionFailedException) } );
		}
	}
}