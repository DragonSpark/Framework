using DragonSpark.Application.AspNet.Entities;
using DragonSpark.Application.AspNet.Entities.Editing;

namespace DragonSpark.Server.Mobile.Platforms.iOS.Attestation.Records;

sealed class Edit<T> : EditingOrDefault<ExistingAttestationRecordInput, T> 
    where T : class, IAttestationRecord
{
    public Edit(IScopes scopes) : base(scopes, SelectAttestationRecord<T>.Default) {}
}