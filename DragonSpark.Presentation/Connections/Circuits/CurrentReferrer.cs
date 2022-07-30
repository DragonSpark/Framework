using DragonSpark.Application.Communication;
using DragonSpark.Application.Security;
using DragonSpark.Compose;
using DragonSpark.Model.Results;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Presentation.Connections.Circuits;

sealed class CurrentReferrer : SelectedResult<IHeaderDictionary, string?>
{
	public CurrentReferrer(ICurrentContext previous)
		: base(previous.Then().Select(x => x.Request.Headers).Get(), RefererHeader.Default) {}
}