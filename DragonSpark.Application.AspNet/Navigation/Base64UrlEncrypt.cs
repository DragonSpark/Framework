using DragonSpark.Application.AspNet.Security.Data;
using DragonSpark.Compose;
using DragonSpark.Model.Selection.Alterations;

namespace DragonSpark.Application.AspNet.Navigation;

public sealed class Base64UrlEncrypt : Alteration<string>
{
	public Base64UrlEncrypt(IEncryptText select) : base(select.Then().Select(Base64UrlEncode.Default)) {}
}