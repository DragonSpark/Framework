using System.Security.Cryptography.X509Certificates;

namespace DragonSpark.Application.Security.Data
{
	sealed class EncryptionCertificate : DragonSpark.Model.Results.Instance<X509Certificate2>
	{
		public EncryptionCertificate(EncryptionSettings settings)
			: base(new X509Certificate2(settings.CertificatePath, settings.Password)) {}
	}
}