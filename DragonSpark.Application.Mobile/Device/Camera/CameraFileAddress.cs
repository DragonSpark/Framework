using System;
using System.Threading.Tasks;
using DragonSpark.Model.Operations.Results;
using Windows.Media.Capture;

namespace DragonSpark.Application.Mobile.Device.Camera;

public sealed class CameraFileAddress : IResulting<Uri?>
{
    public static CameraFileAddress Default { get; } = new();

    CameraFileAddress() : this(new()) {}

    readonly CameraCaptureUI _camera;

    public CameraFileAddress(CameraCaptureUI camera) => _camera = camera;

    public async ValueTask<Uri?> Get()
    {
        var file = await _camera.CaptureFileAsync(CameraCaptureUIMode.Photo);
        return file is not null ? new(file.Path) : null;
    }
}