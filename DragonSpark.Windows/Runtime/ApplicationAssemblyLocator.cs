using System;
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
			var result = Assembly.Load( AppDomain.CurrentDomain.FriendlyName ) ?? base.CreateItem();
			return result;
		}
	}
}