using DragonSpark.Application.AspNet.Entities;
using DragonSpark.Application.AspNet.Entities.Editing;

namespace DragonSpark.Server.Mobile.Platforms.Android.Attestation.Records;

sealed class AddRecord<T> : Updating<NewAttestationRecordInput, T> where T : class, IVerificationRecord
{
    public AddRecord(IScopes scopes, NewRecord<T> @new) : base(scopes, @new) {}
}