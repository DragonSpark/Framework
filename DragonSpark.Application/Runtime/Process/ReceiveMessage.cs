using DragonSpark.Compose;
using DragonSpark.Model.Operations.Results.Stop;
using System.IO;
using System.IO.Pipes;
using System.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Application.Runtime.Process;

/// <summary>
/// ATTRIBUTION: https://stackoverflow.com/a/60546519
/// </summary>
public class ReceiveMessage<T> : ReceiveMessage
{
    protected ReceiveMessage() : base(A.Type<T>().AssemblyQualifiedName.Verify()) {}
}

/// <summary>
/// ATTRIBUTION: https://stackoverflow.com/a/60546519
/// </summary>
public class ReceiveMessage : IStopAware<string>
{
    readonly string       _name;
    readonly PipeSecurity _security;

    protected ReceiveMessage(string name) : this(name, ComposePipeSecurity.Default.Get()) {}

    protected ReceiveMessage(string name, PipeSecurity security)
    {
        _name     = name;
        _security = security;
    }

    public async ValueTask<string> Get(CancellationToken parameter)
    {
        await using var stream = NamedPipeServerStreamAcl.Create(_name, PipeDirection.InOut, 1,
                                                                 PipeTransmissionMode.Message, PipeOptions.Asynchronous,
                                                                 4096, 4096, _security);
        await stream.WaitForConnectionAsync(parameter).Off();

        using var reader = new StreamReader(stream);
        return await reader.ReadToEndAsync(parameter).Off();
    }
}