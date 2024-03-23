using DragonSpark.Application.Security.Data;
using DragonSpark.Compose;
using DragonSpark.Model.Selection.Alterations;
using JetBrains.Annotations;

namespace DragonSpark.Application.Navigation;

[UsedImplicitly]
public sealed class UrlDecrypt : Alteration<string>
{
	public UrlDecrypt(IDecryptText select) : base(UrlDecode.Default.Then().Select(select)) {}
}