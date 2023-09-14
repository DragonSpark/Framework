using DragonSpark.Compose;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection.Alterations;
using Microsoft.AspNetCore.DataProtection;

namespace DragonSpark.Application.Security.Data;

sealed class DecryptText : Alteration<string>, IDecryptText
{
	public DecryptText(IDataProtector protector) : base(protector.Unprotect) {}
}

// TODO

sealed class DataProtectorInstance : IResult<IDataProtector>
{
	readonly IDataProtectionProvider _provider;
	readonly string                  _purpose;

	public DataProtectorInstance(IDataProtectionProvider provider)
		: this(provider, typeof(DataProtectorInstance).Namespace.Verify()) {}

	public DataProtectorInstance(IDataProtectionProvider provider, string purpose)
	{
		_provider = provider;
		_purpose  = purpose;
	}

	public IDataProtector Get() => _provider.CreateProtector(_purpose);
}