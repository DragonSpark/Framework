using DragonSpark.Application.Security;
using DragonSpark.Compose;
using DragonSpark.Model.Results;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Application.Communication;

public class CurrentHeader : SelectedResult<IHeaderDictionary, string?>
{
	protected CurrentHeader(ICurrentContext context, IHeader header)
		: this(context.Then().Select(x => x.Request.Headers).Get(), header) {}

	protected CurrentHeader(IResult<IHeaderDictionary> source, IHeader header) : base(source, header) {}
}