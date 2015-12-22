using DragonSpark.Activation.FactoryModel;
using DragonSpark.Runtime.Specifications;
using DragonSpark.Runtime.Values;
using Microsoft.Practices.ServiceLocation;
using System.Linq;

namespace DragonSpark.Activation
{
	public class AmbientKeyFactory : FactoryBase<IServiceLocator, IAmbientKey>
	{
		public static AmbientKeyFactory Instance { get; } = new AmbientKeyFactory();

		public AmbientKeyFactory() : base( FixedFactoryParameterCoercer<IServiceLocator>.Instance )
		{}

		protected override IAmbientKey CreateItem( IServiceLocator parameter )
		{
			var items = new[] { parameter, null }.Select( item => new EqualityContextAwareSpecification( item ) ).Cast<ISpecification>();
			var specification = new AnySpecification( items.ToArray() );
			var result = new AmbientKey<IServiceLocator>( specification );
			return result;
		}
	}
}