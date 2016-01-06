using System;
using System.Reflection;
using DragonSpark.TypeSystem;

namespace DragonSpark.Windows.Runtime
{
	public class AssemblyProvider : AggregateAssemblyFactory
	{
		public static AssemblyProvider Instance { get; } = new AssemblyProvider();

		public AssemblyProvider() : this( FileSystemAssemblySource.Instance.Create )
		{}

		public AssemblyProvider( Func<Assembly[]> source ) : base( source, ApplicationAssemblyTransformer.Instance.Create )
		{}
	}
}