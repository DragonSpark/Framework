using DragonSpark.Model;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Conditions;

namespace DragonSpark.Application.Mobile.Attestation;

public sealed class ClearValidationState : DependingOnAll<Stop<None>>
{
    public ClearValidationState(IClearClientKey first, IClearValidationIdentity second) : base(first, second) {}
}