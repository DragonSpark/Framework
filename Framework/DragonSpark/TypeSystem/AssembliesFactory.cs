using System.Reflection;
using DragonSpark.Activation.FactoryModel;
using DragonSpark.Setup.Registration;

namespace DragonSpark.TypeSystem
{
	[RegisterFactoryForResult]
	public class AssembliesFactory : FactoryBase<Assembly[]>
	{
		readonly IAssemblyProvider provider;

		public AssembliesFactory( IAssemblyProvider provider )
		{
			this.provider = provider;
		}

		protected override Assembly[] CreateItem()
		{
			return provider.GetAssemblies();
		}
	}
}