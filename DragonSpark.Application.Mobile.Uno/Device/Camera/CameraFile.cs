using System;
using System.Threading.Tasks;
using DragonSpark.Application.Mobile.Uno.Presentation;
using DragonSpark.Compose;
using DragonSpark.Model.Operations.Results;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Uno.Extensions;

namespace DragonSpark.Application.Mobile.Uno.Device.Camera;

public sealed class CameraFile : IResulting<ImageSource?>
{
    readonly IResulting<Uri?> _address;

    public CameraFile(IDispatcher dispatcher) : this(CameraFileAddress.Default.Using(dispatcher)) {}

    public CameraFile(IResulting<Uri?> address) => _address = address;

    public async ValueTask<ImageSource?> Get()
    {
        var address = await _address.Off();
        var result  = address is not null ? new BitmapImage(address) : null;
        return result;
    }
}