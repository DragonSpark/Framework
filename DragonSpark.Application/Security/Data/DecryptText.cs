using DragonSpark.Model.Selection.Alterations;
using Microsoft.AspNetCore.DataProtection;

namespace DragonSpark.Application.Security.Data
{
	sealed class DecryptText : Alteration<string>, IDecryptText
	{
		public DecryptText(CertificateBasedDataProtector protector) : base(protector.Unprotect) {}
	}
}