using System.Linq;
using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Contracts.General;
using DragonSpark.Diagnostics.Logging;
using DragonSpark.Model.Operations;
using Microsoft.Extensions.Logging;
using Exception = System.Exception;

namespace DragonSpark.Grok.Chat;

sealed class ExceptionAwareChatResult : IChatResult
{
    readonly IChatResult _previous;
    readonly Warning     _warning;

    public ExceptionAwareChatResult(IChatResult previous, Warning warning)
    {
        _previous = previous;
        _warning  = warning;
    }

    public sealed class Warning : LogWarningException<string>
    {
        public Warning(ILogger<Warning> logger)
            : base(logger, "A problem occurred when completing the chat with {Message}") {}
    }

    public async Task<ChatMessage> Get(Stop<ChatModelInput> parameter)
    {
        try
        {
            return await _previous.Off(parameter);
        }
        catch (Exception e)
        {
            _warning.Execute(new(e, parameter.Subject.Messages.Last().Content));
            return new("assistant", "Sorry, something went wrong — try again?");
        }
    }
}