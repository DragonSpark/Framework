using System.Linq;
using System.Windows;
using System.Windows.Controls;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.Converters
{
	public class ErrorConverter : ValueConverterBase<FrameworkElement,object>
	{
		public static ErrorConverter Instance
		{
			get { return InstanceField; }
		}	static readonly ErrorConverter InstanceField = new ErrorConverter();

		protected override object PerformConversion( FrameworkElement value, object parameter )
		{
			var result =
				Validation.GetErrors( value ).FirstOrDefault().Transform(
					item => item.Exception.InnerException.Transform( inner => inner.Message, () => item.ErrorContent ) );
			return result;
		}
	}
}