using DragonSpark.Application.Security.Data;
using DragonSpark.Compose;
using DragonSpark.Model.Selection.Alterations;

namespace DragonSpark.Application.Navigation;

public sealed class UrlEncrypt : Alteration<string>
{
	public UrlEncrypt(IEncryptText select) : base(select.Then().Select(UrlEncode.Default)) {}
}