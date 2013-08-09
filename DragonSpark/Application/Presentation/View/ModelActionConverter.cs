using System.Linq;
using DragonSpark.Application.Presentation.ComponentModel;
using DragonSpark.Application.Presentation.Converters;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.View
{
	public class ModelActionConverter : ValueConverterBase<object,object>
	{
		public static ModelActionConverter Instance
		{
			get { return InstanceField; }
		}	static readonly ModelActionConverter InstanceField = new ModelActionConverter();

		protected override object PerformConversion( object value, object parameter )
		{
			var query = value.Transform( x => x.GetType().GetMethods().Select( y => y.FromMetadata<ModelActionAttribute, ModelActionDescriptor>( z => new ModelActionDescriptor { ActionName = y.Name, Description = z.Description } ) ).NotNull() );
			var result = query.Transform( x => new ViewCollection<ModelActionDescriptor>( x ) );
			return result;
		}
	}
}