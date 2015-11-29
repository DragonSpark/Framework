using DragonSpark.Activation;
using DragonSpark.Runtime;
using Microsoft.Practices.ServiceLocation;
using System.Reflection;

namespace DragonSpark.Testing.Framework
{
	public class AmbientLocatorKeyFactory : FactoryBase<MethodInfo, IAmbientKey>
	{
		public static AmbientLocatorKeyFactory Instance { get; } = new AmbientLocatorKeyFactory();

		protected override IAmbientKey CreateItem( MethodInfo parameter )
		{
			var specification = new CurrentMethodSpecification( parameter ).Or( new CurrentTaskSpecification() );
			var result = new AmbientKey<IServiceLocator>( specification );
			return result;
		}
	}
}