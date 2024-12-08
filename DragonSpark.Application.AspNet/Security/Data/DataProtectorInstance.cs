using DragonSpark.Compose;
using DragonSpark.Model.Results;
using Microsoft.AspNetCore.DataProtection;

namespace DragonSpark.Application.AspNet.Security.Data;

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