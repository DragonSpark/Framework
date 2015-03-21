using DragonSpark.Extensions;
using Prism.Modularity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DragonSpark.Application.Runtime
{
	public class AssemblyProvider : Activation.AssemblyProvider	
	{
		public static AssemblyProvider Instance
		{
			get { return InstanceField; }
		}	static readonly AssemblyProvider InstanceField = new AssemblyProvider();

		readonly IAssemblyProvider provider;

		public AssemblyProvider() : this( new Prism.Unity.Windows.AssemblyProvider() )
		{}

		public AssemblyProvider( IAssemblyProvider provider )
		{
			this.provider = provider;
		}

		public AssemblyProvider( Func<Assembly, bool> filter ) : base( filter )
		{}

		protected override Assembly[] DetermineAllAssemblies()
		{
			return provider.GetAssemblies().ToArray();
		}

		protected override IEnumerable<Assembly> DetermineCoreAssemblies()
		{
			var result = base.DetermineCoreAssemblies().Append( Assembly.GetExecutingAssembly() );
			return result;
		}
	}
}