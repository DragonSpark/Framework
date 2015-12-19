using System;
using System.IO;
using System.Reflection;
using DragonSpark.Setup.Registration;

namespace DragonSpark.Windows.Runtime
{
	[RegisterFactoryForResult]
	public class ApplicationAssemblyLocator : TypeSystem.ApplicationAssemblyLocator
	{
		public ApplicationAssemblyLocator( Assembly[] assemblies ) : base( assemblies )
		{}

		protected override Assembly CreateItem()
		{
			var result = DeterminePrimaryAssembly() ?? base.CreateItem();
			return result;
		}

		static Assembly DeterminePrimaryAssembly()
		{
			try
			{
				return Assembly.Load( AppDomain.CurrentDomain.FriendlyName );
			}
			catch ( FileNotFoundException )
			{
				var result = Assembly.GetEntryAssembly();
				return result;
			}
		}
	}
}