using System.Text.Json;
using DragonSpark.Compose;
using DragonSpark.Model.Operations.Selection;
using DragonSpark.Model.Sequences;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication.Additional;

sealed class FormatUserData<T> : IFormatUserData<T> where T : class
{
    readonly ISelecting<ComposeUserDataInput<T>, IReadOnlyDictionary<string, string>> _compose;

    public FormatUserData(IProperties properties) : this(new ComposeUserData<T>(properties)) {}

    public FormatUserData(ISelecting<ComposeUserDataInput<T>, IReadOnlyDictionary<string, string>> compose)
        => _compose = compose;

    public async ValueTask<Array<byte>> Get(ComposeUserDataInput<T> parameter)
    {
        var data   = await _compose.Off(parameter);
        var result = JsonSerializer.SerializeToUtf8Bytes(data);
        return result;
    }
}