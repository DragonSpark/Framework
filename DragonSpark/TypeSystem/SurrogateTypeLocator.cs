using System;
using System.Linq;
using DragonSpark.Activation.FactoryModel;
using DragonSpark.Extensions;

namespace DragonSpark.TypeSystem
{
	public class SurrogateTypeLocator : FactoryBase<object, Type>
	{
		public static SurrogateTypeLocator Instance { get; } = new SurrogateTypeLocator();

		protected override Type CreateItem( object parameter )
		{
			var result =
				parameter.AsTo<ISurrogate, Type>( surrogate => surrogate.For )
				??
				parameter.FromMetadata<SurrogateForAttribute, Type>( attribute => attribute.Type )
				??
				parameter.GetType().With( type => type.Name.Replace( nameof(Attribute), string.Empty ).With( name => type.Assembly().DefinedTypes.AsTypes().Where( info => info.Name == name ).Only() ) );
			return result;
		}
	}
}