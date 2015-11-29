using DragonSpark.Application.Client.Converters;
using Xamarin.Forms;

namespace DragonSpark.Application.Client.Forms.Rendering
{
	public class RenderingConverter : ConverterBase<Page>
	{
		public static RenderingConverter Instance
		{
			get { return InstanceField; }
		}	static readonly RenderingConverter InstanceField = new RenderingConverter();

		protected override object PerformConversion( Page value, object parameter )
		{
			var result = value.DetermineRenderer();
			return result;
		}
	}
}
