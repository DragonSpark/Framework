using System;
using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;
using DragonSpark.Server.Mobile.Platforms.iOS.Assertion;

namespace DragonSpark.Server.Mobile.Platforms.iOS.Attestation.Records;

sealed class ExistingAttestation<T> : IExistingAttestation where T : class, IAttestationRecord
{
    readonly Edit<T>                               _edit;
    readonly ISelect<AssertionCounterInput, uint?> _count;

    public ExistingAttestation(Edit<T> edit) : this(edit, AssertionCounter.Default) {}

    public ExistingAttestation(Edit<T> edit, ISelect<AssertionCounterInput, uint?> count)
    {
        _edit  = edit;
        _count = count;
    }

    public async ValueTask<Guid?> Get(Stop<ExistingAttestationRecordInput> parameter)
    {
        using var edit = await _edit.Off(parameter);
        if (edit.Subject is not null)
        {
            var ((_, payload, _, challenge), _) = parameter;
            var count = _count.Get(new(challenge, payload, edit.Subject));
            if (count is not null)
            {
                edit.Subject.Count = count.Value;
                await edit.Off();
                return edit.Subject.Identity;
            }
        }

        return null;
    }
}