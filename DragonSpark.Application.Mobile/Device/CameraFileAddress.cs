using System;
using System.Threading.Tasks;
using DragonSpark.Model.Operations.Results;
using Windows.Media.Capture;

namespace DragonSpark.Application.Mobile.Device;

public sealed class CameraFileAddress : IResulting<Uri?>
{
    public static CameraFileAddress Default { get; } = new();

    CameraFileAddress() {}

    public async ValueTask<Uri?> Get()
    {
        var camera = new CameraCaptureUI { PhotoSettings = { Format = CameraCaptureUIPhotoFormat.Jpeg } };
        var file   = await camera.CaptureFileAsync(CameraCaptureUIMode.Photo);
        return file is not null ? new(file.Path) : null;
    }
}