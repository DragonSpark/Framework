using DragonSpark.Application.AspNet.Entities;
using DragonSpark.Application.AspNet.Entities.Editing;

namespace DragonSpark.Server.Mobile.Platforms.iOS.Attestation.Records;

sealed class AddRecord<T> : Updating<Attestation, T> where T : class, IAttestationRecord
{
    public AddRecord(IScopes scopes) : base(scopes, NewRecord<T>.Default) {}
}