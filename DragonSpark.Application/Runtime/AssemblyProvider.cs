using DragonSpark.Extensions;
using System.Collections.Generic;
using System.Reflection;

namespace DragonSpark.Application.Runtime
{
	public class AssemblyProvider : Activation.AssemblyProvider	
	{
		public static AssemblyProvider Instance
		{
			get { return InstanceField; }
		}	static readonly AssemblyProvider InstanceField = new AssemblyProvider();

		public AssemblyProvider() : base( Prism.Unity.Windows.AssemblyProvider.Instance )
		{}

		protected override IEnumerable<Assembly> DetermineCoreAssemblies()
		{
			var result = base.DetermineCoreAssemblies().Append( Assembly.GetExecutingAssembly() );
			return result;
		}
	}
}