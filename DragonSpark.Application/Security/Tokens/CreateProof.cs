using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;
using DragonSpark.Model.Sequences.Memory;

namespace DragonSpark.Application.Security.Tokens;

sealed class CreateProof : IStopAware<CreateProofInput, string>
{
    readonly SigningInput      _input;
    readonly Signature         _signature;
    readonly INewLeasing<char> _leasing;

    public CreateProof(SigningInput input, Signature signature) : this(input, signature, NewLeasing<char>.Default) {}

    public CreateProof(SigningInput input, Signature signature, INewLeasing<char> leasing)
    {
        _input     = input;
        _signature = signature;
        _leasing   = leasing;
    }

    public async ValueTask<string> Get(Stop<CreateProofInput> parameter)
    {
        var (_, stop) = parameter;

        var       input     = await _input.Off(parameter);
        using var signature = await _signature.Off(new(input, stop));
        using var buffer    = _leasing.Get((uint)(input.Length + 1 + signature.Length));
        var       span      = buffer.AsSpan();
        input.AsSpan().CopyTo(span);
        span[input.Length] = '.';
        signature.AsSpan().CopyTo(span[(input.Length + 1)..]);
        return new(span);
    }
}