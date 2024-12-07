using DragonSpark.Model.Selection.Alterations;
using Microsoft.AspNetCore.DataProtection;

namespace DragonSpark.Application.Security.Data;

sealed class EncryptText : Alteration<string>, IEncryptText
{
	public EncryptText(IDataProtector protector) : base(protector.Protect) {}
}