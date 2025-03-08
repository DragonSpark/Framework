using System;
using System.IO;
using System.IO.Pipes;
using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Allocated;

namespace DragonSpark.Application.Runtime.Process;

/// <summary>
/// ATTRIBUTION: https://stackoverflow.com/a/60546519
/// </summary>
public class SendMessage : ITokenOperation<string>
{
    readonly string   _name;
    readonly TimeSpan _timeout;

    protected SendMessage(string name) : this(name, TimeSpan.FromMilliseconds(100)) {}

    protected SendMessage(string name, TimeSpan timeout)
    {
        _name    = name;
        _timeout = timeout;
    }

    public async ValueTask Get(Token<string> parameter)
    {
        var (subject, item) = parameter;
        await using var client = new NamedPipeClientStream(_name);

        await client.ConnectAsync(_timeout, item).Off();

        if (!client.IsConnected)
        {
            throw new InvalidOperationException("Could not connect");
        }

        await using var writer = new StreamWriter(client);
        await writer.WriteAsync(subject.AsMemory(), item).Off();
        await writer.FlushAsync(item).Off();
    }
}

/// <summary>
/// ATTRIBUTION: https://stackoverflow.com/a/60546519
/// </summary>
public class SendMessage<T> : SendMessage
{
    protected SendMessage() : base(A.Type<T>().AssemblyQualifiedName.Verify()) {}
}