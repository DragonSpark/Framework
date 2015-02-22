using System.Windows.Media;
using Xamarin.Forms;

namespace DragonSpark.Application.Forms.Rendering
{
	internal static class ImageExtensions
	{
		public static Stretch ToStretch(this Aspect aspect)
		{
			switch (aspect)
			{
			case Aspect.AspectFill:
				return Stretch.UniformToFill;
			case Aspect.Fill:
				return Stretch.Fill;
			}
			return Stretch.Uniform;
		}
	}
}
