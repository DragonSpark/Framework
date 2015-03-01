using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DragonSpark.Application.Client.Forms.Rendering
{
	public interface IImageSourceHandler : IRegisterable
	{
		Task<System.Windows.Media.ImageSource> LoadImageAsync(ImageSource imagesoure, CancellationToken cancelationToken = default(CancellationToken));
	}
}
