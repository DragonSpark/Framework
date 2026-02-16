using DragonSpark.Model;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Conditions;

namespace DragonSpark.Application.Mobile.Attestation;

public interface IClearClientKey : DragonSpark.Model.Operations.Selection.Stop.IDepending;

// TODO

public sealed class Clear : DependingOnAll<Stop<None>>
{
    public Clear(IClearClientKey first, IClearValidationIdentity second) : base(first, second) {}
}