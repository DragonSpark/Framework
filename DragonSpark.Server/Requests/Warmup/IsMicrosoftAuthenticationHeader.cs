using DragonSpark.Application.AspNet.Communication;
using DragonSpark.Application.Communication;
using DragonSpark.Model.Selection.Conditions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using System;

namespace DragonSpark.Server.Requests.Warmup;

[UsedImplicitly]
sealed class IsMicrosoftAuthenticationHeader : ICondition<IHeaderDictionary>
{
	public static IsMicrosoftAuthenticationHeader Default { get; } = new();

	IsMicrosoftAuthenticationHeader() : this(MicrosoftAuthenticationHeader.Default, bool.TrueString) { }

	readonly IHeader _header;
	readonly string  _expected;

	public IsMicrosoftAuthenticationHeader(IHeader header, string expected)
	{
		_header   = header;
		_expected = expected;
	}

	public bool Get(IHeaderDictionary parameter)
		=> string.Equals(_header.Get(parameter), _expected, StringComparison.CurrentCultureIgnoreCase);
}