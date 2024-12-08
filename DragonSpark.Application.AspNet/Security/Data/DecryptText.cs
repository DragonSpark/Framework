using DragonSpark.Model.Selection.Alterations;
using Microsoft.AspNetCore.DataProtection;

namespace DragonSpark.Application.AspNet.Security.Data;

sealed class DecryptText : Alteration<string>, IDecryptText
{
	public DecryptText(IDataProtector protector) : base(protector.Unprotect) {}
}