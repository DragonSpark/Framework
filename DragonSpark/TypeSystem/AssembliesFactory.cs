using System.Reflection;
using DragonSpark.Activation.FactoryModel;
using DragonSpark.Setup.Registration;

namespace DragonSpark.TypeSystem
{
	[Register.Factory]
	public class AssembliesFactory : FactoryBase<Assembly[]>
	{
		readonly IAssemblyProvider provider;

		public AssembliesFactory( IAssemblyProvider provider )
		{
			this.provider = provider;
		}

		protected override Assembly[] CreateItem() => provider.Create();
	}
}