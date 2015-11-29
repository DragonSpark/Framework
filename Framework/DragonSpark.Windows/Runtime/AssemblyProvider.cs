using System;
using DragonSpark.Extensions;
using DragonSpark.Runtime;
using DragonSpark.Setup;
using System.Collections.Generic;
using System.Reflection;

namespace DragonSpark.Windows.Runtime
{
	public class AssemblyProvider : FilteredAssemblyProviderBase
	{
		public static AssemblyProvider Instance { get; } = new AssemblyProvider();

		public AssemblyProvider() : base( FileSystemAssemblyProvider.Instance )
		{}

		protected override IEnumerable<Assembly> DetermineCoreAssemblies()
		{
			var assembly = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();
			var result = base.DetermineCoreAssemblies().Append( assembly, typeof(AssemblyProvider).Assembly );
			return result;
		}
	}

	[RegisterFactoryForResult]
	public class ApplicationAssemblyLocator : DragonSpark.Runtime.ApplicationAssemblyLocator
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