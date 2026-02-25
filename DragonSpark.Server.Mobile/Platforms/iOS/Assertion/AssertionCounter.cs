using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Model.Sequences;

namespace DragonSpark.Server.Mobile.Platforms.iOS.Assertion;

public sealed class AssertionCounter : ISelect<AssertionCounterInput, uint?>
{
    public static AssertionCounter Default { get; } = new();

    AssertionCounter() : this(VerifyPublicKey.Default, GetAssertionPayload.Default, DetermineCount.Default) {}

    readonly ICondition<VerifyPublicKeyInput>    _expected;
    readonly IArray<AssertionPayloadInput, byte> _payload;
    readonly ISelect<Array<byte>, uint?>         _count;

    public AssertionCounter(ICondition<VerifyPublicKeyInput> expected, IArray<AssertionPayloadInput, byte> payload,
                            ISelect<Array<byte>, uint?> count)
    {
        _expected = expected;
        _payload  = payload;
        _count    = count;
    }

    public uint? Get(AssertionCounterInput parameter)
    {
        var ((challenge, payload), record) = parameter;
        if (_expected.Get(new(record.PublicKeyHash, record.PublicKey)))
        {
            var bytes = _payload.Get(new(payload, record.PublicKey, challenge));
            if (bytes.Length > 0)
            {
                var result = _count.Get(bytes);
                if (result > record.Count)
                {
                    return result;
                }
            }
        }

        return null;
    }
}