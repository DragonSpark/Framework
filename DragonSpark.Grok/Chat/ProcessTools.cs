using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Contracts.General.Chat;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.Grok.Chat;

sealed class ProcessTools : IStopAware<ChatMessage, IReadOnlyList<ChatMessage>>
{
    readonly IExecuteTools _execute;

    public ProcessTools(IExecuteTools execute) => _execute = execute;

    public async ValueTask<IReadOnlyList<ChatMessage>> Get(Stop<ChatMessage> parameter)
    {
        var (message, stop) = parameter;
        var result = new List<ChatMessage>();
        if (message is AssistantMessage assistant && assistant.ToolCalls?.Any() == true)
        {
            foreach (var toolCall in assistant.ToolCalls)
            {
                if (toolCall is FunctionToolCall ftc)
                {
                    var content = await _execute.Off(new(new(ftc.Function.Name, ftc.Function.Arguments), stop));
                    result.Add(new ToolMessage(ftc.Id, content));
                }
            }
        }
        else if (message is TextMessage text)
        {
            result.Add(text);
        }

        return result;
    }
}