using DragonSpark.Activation;
using DragonSpark.Activation.FactoryModel;
using DragonSpark.Setup.Registration;
using DragonSpark.TypeSystem;
using System;
using System.Linq;

namespace DragonSpark.Setup
{
	[RegisterType]
	public class AllTypesOfFactory : FactoryBase<Type, Array>
	{
		readonly IAssemblyProvider provider;
		readonly IActivator activator;

		public AllTypesOfFactory( IAssemblyProvider provider, IActivator activator )
		{
			this.provider = provider;
			this.activator = activator;
		}

		public T[] Create<T>() => Create( typeof(T) ).Cast<T>().ToArray();

		protected override Array CreateItem( Type parameter )
		{
			var types = provider.Create().SelectMany( assembly => assembly.ExportedTypes );
			var result = activator.ActivateMany( parameter, types ).ToArray();
			return result;
		}
	}
}