using System.Security.Cryptography;

namespace DragonSpark.Xamarin.Forms.Platform.Wpf.Rendering
{
	internal abstract class MD5 : HashAlgorithm
	{
		public MD5()
		{
			this.HashSizeValue = 128;
		}
	}
}
