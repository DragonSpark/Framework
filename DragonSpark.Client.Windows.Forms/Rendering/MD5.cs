using System.Security.Cryptography;

namespace DragonSpark.Client.Windows.Forms.Rendering
{
	internal abstract class MD5 : HashAlgorithm
	{
		public MD5()
		{
			this.HashSizeValue = 128;
		}
	}
}
