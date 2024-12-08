using DragonSpark.Model.Selection.Alterations;
using Microsoft.AspNetCore.DataProtection;

namespace DragonSpark.Application.AspNet.Security.Data;

sealed class Decrypt : Alteration<byte[]>, IDecrypt
{
	public Decrypt(IDataProtector protector) : base(protector.Unprotect) {}
}