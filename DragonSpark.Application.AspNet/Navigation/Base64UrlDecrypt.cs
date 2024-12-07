using DragonSpark.Application.Security.Data;
using DragonSpark.Compose;
using DragonSpark.Model.Selection.Alterations;

namespace DragonSpark.Application.Navigation;

public sealed class Base64UrlDecrypt : Alteration<string>
{
	public Base64UrlDecrypt(IDecryptText select) : base(Base64UrlDecode.Default.Then().Select(select)) {}
}