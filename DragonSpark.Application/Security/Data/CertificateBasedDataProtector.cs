using DragonSpark.Compose;
using Microsoft.AspNetCore.DataProtection;
using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace DragonSpark.Application.Security.Data
{
	sealed class CertificateBasedDataProtector : IDataProtector
	{
		readonly X509Certificate2     _certificate;
		readonly RSAEncryptionPadding _padding;

		public CertificateBasedDataProtector(X509Certificate2 certificate)
			: this(certificate, RSAEncryptionPadding.OaepSHA1) {}

		public CertificateBasedDataProtector(X509Certificate2 certificate, RSAEncryptionPadding padding)
		{
			_certificate = certificate;
			_padding     = padding;
		}

		public IDataProtector CreateProtector(string purpose) => throw new NotSupportedException();

		public byte[] Protect(byte[] plaintext)
		{
			using var rsa    = _certificate.GetRSAPublicKey().Verify();
			var       result = rsa.Encrypt(plaintext, _padding);
			return result;
		}

		public byte[] Unprotect(byte[] protectedData)
		{
			using var rsa    = _certificate.GetRSAPrivateKey().Verify();
			var       result = rsa.Decrypt(protectedData, _padding);
			return result;
		}
	}
}