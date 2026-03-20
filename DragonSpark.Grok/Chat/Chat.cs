using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Contracts.General.Chat;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Sequences;

namespace DragonSpark.Grok.Chat;

sealed class Chat : IChat
{
    readonly IChatResponse _response;
    readonly ProcessTools  _process;
    readonly Array<Tool>   _tools;

    public Chat(IChatResponse response, ProcessTools process, Tools tools) : this(response, process, tools.Get()) {}

    public Chat(IChatResponse response, ProcessTools process, Array<Tool> tools)
    {
        _response = response;
        _process  = process;
        _tools    = tools;
    }

    public async Task<ImmutableArray<ChatMessage>> Get(Stop<ChatModelInput> parameter)
    {
        var (input, stop)       = parameter;
        var (_, messages, _, _) = input;
        var message = await _response.Off(new(new(input, _tools), stop));
        var append  = await _process.Off(new(message, stop));
        return [..messages.Where(m => m is not SystemMessage).Concat(append)];
    }
}