using DragonSpark.Setup.Registration;
using Microsoft.Practices.Unity;
using System;
using System.IO;
using System.Reflection;
using DragonSpark.Activation.FactoryModel;
using DragonSpark.TypeSystem;
using PostSharp.Patterns.Contracts;

namespace DragonSpark.Windows.Runtime
{
	[RegisterFactory]
	public class ApplicationAssemblyLocator : FirstFactory<Assembly>
	{
		public ApplicationAssemblyLocator( DomainApplicationAssemblyLocator domain, TypeSystem.ApplicationAssemblyLocator system ) : base( domain, system ) {}
	}

	public class DomainApplicationAssemblyLocator : FactoryBase<Assembly>, IApplicationAssemblyLocator
	{
		public static DomainApplicationAssemblyLocator Instance { get; } = new DomainApplicationAssemblyLocator();

		readonly AppDomain primary;

		[InjectionConstructor]
		public DomainApplicationAssemblyLocator() : this( AppDomain.CurrentDomain ) {}

		public DomainApplicationAssemblyLocator( [Required]AppDomain primary ) 
		{
			this.primary = primary;
		}

		protected override Assembly CreateItem()
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