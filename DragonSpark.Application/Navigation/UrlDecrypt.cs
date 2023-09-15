using DragonSpark.Application.Security.Data;
using DragonSpark.Compose;
using DragonSpark.Model.Selection.Alterations;

namespace DragonSpark.Application.Navigation;

public sealed class UrlDecrypt : Alteration<string>
{
	public UrlDecrypt(IDecryptText select) : base(UrlDecode.Default.Then().Select(select)) {}
}