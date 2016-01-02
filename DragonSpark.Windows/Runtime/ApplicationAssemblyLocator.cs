using DragonSpark.Setup.Registration;
using Microsoft.Practices.Unity;
using System;
using System.IO;
using System.Reflection;

namespace DragonSpark.Windows.Runtime
{
	[RegisterFactoryForResult]
	public class ApplicationAssemblyLocator : TypeSystem.ApplicationAssemblyLocator
	{
		readonly AppDomain primary;

		[InjectionConstructor]
		public ApplicationAssemblyLocator( Assembly[] assemblies ) : this( assemblies, AppDomain.CurrentDomain )
		{}

		public ApplicationAssemblyLocator( Assembly[] assemblies, AppDomain primary ) : base( assemblies )
		{
			this.primary = primary;
		}

		protected override Assembly CreateItem()
		{
			var result = DeterminePrimaryAssembly() ?? base.CreateItem();
			return result;
		}

		Assembly DeterminePrimaryAssembly()
		{
			try
			{
				return Assembly.Load( primary.FriendlyName );
			}
			catch ( FileNotFoundException )
			{
				var result = Assembly.GetEntryAssembly();
				return result;
			}
		}
	}
}