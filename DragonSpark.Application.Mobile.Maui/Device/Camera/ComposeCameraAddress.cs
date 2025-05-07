using System;
using System.IO;
using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Operations.Allocated;
using Microsoft.Maui.Storage;

namespace DragonSpark.Application.Mobile.Maui.Device.Camera;

sealed class ComposeCameraAddress : IAllocating<FileResult, Uri>
{
    public static ComposeCameraAddress Default { get; } = new();

    ComposeCameraAddress() {}

    public async Task<Uri> Get(FileResult parameter)
    {
        // save the file into local storage
        var path = Path.Combine(FileSystem.CacheDirectory, parameter.FileName);
        await using (var stream = await parameter.OpenReadAsync().Off())
        await using (var newStream = File.OpenWrite(path))
        {
            await stream.CopyToAsync(newStream).Off();
        }

        return new($"file:///{path}");
    }
}