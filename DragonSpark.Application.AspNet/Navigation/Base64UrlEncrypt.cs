using DragonSpark.Application.Security.Data;
using DragonSpark.Compose;
using DragonSpark.Model.Selection.Alterations;

namespace DragonSpark.Application.Navigation;

public sealed class Base64UrlEncrypt : Alteration<string>
{
	public Base64UrlEncrypt(IEncryptText select) : base(select.Then().Select(Base64UrlEncode.Default)) {}
}