using DragonSpark.Model.Selection.Alterations;

namespace DragonSpark.Application.Security.Data;

sealed class Decrypt : Alteration<byte[]>, IDecrypt
{
	public Decrypt(CertificateBasedDataProtector protector) : base(protector.Unprotect) {}
}