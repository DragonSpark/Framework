using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DragonSpark.Client.Windows.Forms.Rendering
{
	public interface IImageSourceHandler : IRegisterable
	{
		Task<System.Windows.Media.ImageSource> LoadImageAsync(global::Xamarin.Forms.ImageSource imagesoure, CancellationToken cancelationToken = default(CancellationToken));
	}
}
