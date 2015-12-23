using DragonSpark.Activation.FactoryModel;
using DragonSpark.Runtime.Specifications;
using DragonSpark.Runtime.Values;
using System.Reflection;

namespace DragonSpark.Testing.Framework.Setup
{
	public class AmbientKeyFactory<T> : FactoryBase<MethodInfo, IAmbientKey>
	{
		public static AmbientKeyFactory<T> Instance { get; } = new AmbientKeyFactory<T>();

		protected override IAmbientKey CreateItem( MethodInfo parameter )
		{
			var specification = new CurrentMethodSpecification( parameter ).Or( new CurrentTaskSpecification() );
			var result = new AmbientKey<T>( specification );
			return result;
		}
	}
}