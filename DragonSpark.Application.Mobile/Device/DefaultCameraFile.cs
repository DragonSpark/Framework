using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;

namespace DragonSpark.Application.Mobile.Device;

public sealed class DefaultCameraFile : DragonSpark.Model.Results.Instance<ImageSource>
{
    public static DefaultCameraFile Default { get; } = new();

    DefaultCameraFile() : base(new BitmapImage(new("ms-appx:///Assets/Images/none_selected.png"))) {}
}