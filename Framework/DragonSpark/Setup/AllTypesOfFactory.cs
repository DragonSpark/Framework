using DragonSpark.Activation;
using DragonSpark.Runtime;
using System;
using System.Linq;
using DragonSpark.Activation.FactoryModel;
using DragonSpark.Setup.Registration;
using DragonSpark.TypeSystem;

namespace DragonSpark.Setup
{
	[Register]
	public class AllTypesOfFactory : FactoryBase<Type, Array>
	{
		readonly IAssemblyProvider provider;
		readonly IActivator activator;

		public AllTypesOfFactory( IAssemblyProvider provider, IActivator activator )
		{
			this.provider = provider;
			this.activator = activator;
		}

		protected override Array CreateItem( Type parameter )
		{
			// var type = parameter ?? resultType.Extend().GetInnerType() ?? resultType;
			var types = provider.GetAssemblies().SelectMany( assembly => assembly.ExportedTypes );
			var items = activator.ActivateMany( parameter, types ).ToArray();
			var result = Array.CreateInstance( parameter, items.Length );
			Array.Copy( items, result, items.Length );
			return result;
		}
	}
}