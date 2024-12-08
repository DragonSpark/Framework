using DragonSpark.Model.Selection.Alterations;
using Microsoft.AspNetCore.DataProtection;

namespace DragonSpark.Application.AspNet.Security.Data;

sealed class Encrypt : Alteration<byte[]>, IEncrypt
{
	public Encrypt(IDataProtector protector) : base(protector.Protect) {}
}