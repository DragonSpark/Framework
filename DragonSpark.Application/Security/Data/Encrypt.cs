using DragonSpark.Model.Selection.Alterations;

namespace DragonSpark.Application.Security.Data
{
	sealed class Encrypt : Alteration<byte[]>, IEncrypt
	{
		public Encrypt(CertificateBasedDataProtector protector) : base(protector.Protect) {}
	}
}