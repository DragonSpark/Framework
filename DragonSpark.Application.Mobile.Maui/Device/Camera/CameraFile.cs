using System;
using System.Threading.Tasks;
using DragonSpark.Application.Mobile.Maui.Presentation;
using DragonSpark.Compose;
using DragonSpark.Model.Operations.Results;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Dispatching;

namespace DragonSpark.Application.Mobile.Maui.Device.Camera;

public sealed class CameraFile : IResulting<ImageSource?>
{
    readonly IResulting<Uri?> _address;

    public CameraFile(IDispatcher dispatcher) : this(CameraFileAddress.Default.Using(dispatcher)) {}

    public CameraFile(IResulting<Uri?> address) => _address = address;

    public async ValueTask<ImageSource?> Get()
    {
        var address = await _address.Off();
        var result  = address is not null ? ImageSource.FromUri(address) : null;
        return result;
    }
}