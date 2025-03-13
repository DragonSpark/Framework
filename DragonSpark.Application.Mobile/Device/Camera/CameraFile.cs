using System;
using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Operations.Results;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;

namespace DragonSpark.Application.Mobile.Device.Camera;

public sealed class CameraFile : IResulting<ImageSource?>
{
    public static CameraFile Default { get; } = new();

    CameraFile() : this(CameraFileAddress.Default) {}

    readonly IResulting<Uri?> _address;

    public CameraFile(IResulting<Uri?> address) => _address = address;

    public async ValueTask<ImageSource?> Get()
    {
        var address = await _address.Off();
        var result  = address is not null ? new BitmapImage(address) : null;
        return result;
    }
}