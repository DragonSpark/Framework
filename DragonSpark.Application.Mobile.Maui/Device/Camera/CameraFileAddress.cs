using System;
using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Operations.Allocated;
using DragonSpark.Model.Operations.Results;
using Microsoft.Maui.Media;
using Microsoft.Maui.Storage;

namespace DragonSpark.Application.Mobile.Maui.Device.Camera;

public sealed class CameraFileAddress : IResulting<Uri?>
{
    public static CameraFileAddress Default { get; } = new();

    CameraFileAddress() : this(ComposeCameraAddress.Default) {}

    readonly IAllocating<FileResult, Uri> _address;

    public CameraFileAddress(IAllocating<FileResult, Uri> address) => _address = address;

    public async ValueTask<Uri?> Get()
    {
        var file = await MediaPicker.CapturePhotoAsync().Off();
        return file is not null ? await _address.Off(file) : null;
    }
}