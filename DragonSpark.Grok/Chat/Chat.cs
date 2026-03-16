using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Contracts.General;
using DragonSpark.Model.Operations;

namespace DragonSpark.Grok.Chat;

public sealed class Chat : IChat
{
    readonly IChatResult _result;

    public Chat(IChatResult result) => _result = result;

    public async Task<ImmutableArray<ChatMessage>> Get(Stop<ChatModelInput> parameter)
    {
        var ((messages, _, _, _), _) = parameter;

        var result = await _result.Off(parameter);

        return [..messages.Skip(1).Where(m => m.Role != "system").Append(result)];
    }
}