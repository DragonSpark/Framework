using Microsoft.Maui.Controls;

namespace DragonSpark.Application.Mobile.Maui.Device.Camera;

public sealed class DefaultCameraFile : DragonSpark.Model.Results.Instance<ImageSource>
{
    public static DefaultCameraFile Default { get; } = new();

    DefaultCameraFile() : base(ImageSource.FromFile("none_selected.png")) {}
}