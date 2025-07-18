using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection;
using OpenAI.Images;

namespace DragonSpark.Azure.Ai;

public sealed class GenerateImage : ISelecting<Stop<GenerateImageInput>, GeneratedImage>
{
    readonly ImageClient _client;

    public GenerateImage(ImageClient client) => _client = client;

    public async ValueTask<GeneratedImage> Get(Stop<GenerateImageInput> parameter)
    {
        var ((prompt, size, format), item) = parameter;
        var response = await _client.GenerateImageAsync(prompt, new() { ResponseFormat = format, Size = size }, item)
                                    .Off();
        return response.Value;
    }
}