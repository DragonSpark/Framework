using System.Windows.Media;

namespace DragonSpark.Client.Windows.Forms.Rendering
{
	internal static class ConvertExtensions
	{
		public static System.Windows.Media.Color ToMediaColor(this global::Xamarin.Forms.Color color)
		{
			return System.Windows.Media.Color.FromArgb((byte)(color.A * 255.0), (byte)(color.R * 255.0), (byte)(color.G * 255.0), (byte)(color.B * 255.0));
		}
		public static Brush ToBrush(this global::Xamarin.Forms.Color color)
		{
			return new SolidColorBrush(color.ToMediaColor());
		}
	}
}
