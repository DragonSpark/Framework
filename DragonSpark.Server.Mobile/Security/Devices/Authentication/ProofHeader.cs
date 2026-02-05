using DragonSpark.Application.AspNet.Communication;
using DragonSpark.Application.Security.Tokens;

namespace DragonSpark.Server.Mobile.Security.Devices.Authentication;

sealed class ProofHeader : Header
{
    public static ProofHeader Default { get; } = new();

    ProofHeader() : base(ProofName.Default) {}
}