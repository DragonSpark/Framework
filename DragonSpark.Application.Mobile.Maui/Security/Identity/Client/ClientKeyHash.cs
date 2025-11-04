using DragonSpark.Application.Mobile.Attestation;
using DragonSpark.Model.Operations.Results.Stop;

namespace DragonSpark.Application.Mobile.Maui.Security.Identity.Client;

sealed class ClientKeyHash : ProcessStoring<string>, IClientKeyHash
{
    public ClientKeyHash(IClientKeyHash previous) : base(ClientKeyHashProcessStore.Default, previous) {}
}