using DragonSpark.Compose;
using DragonSpark.Model.Operations.Selection;
using DragonSpark.Model.Selection.Conditions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace DragonSpark.Azure.Storage.Uploads;

public class RequestFileResultBase : ISelecting<Server.Requests.Query<IStorageEntry>, IActionResult>
{
	readonly ICondition<string>                 _streams;
	readonly Func<RequestFileResultInput, bool> _downloadable;

	protected RequestFileResultBase() : this(_ => true) {}

	protected RequestFileResultBase(Func<RequestFileResultInput, bool> downloadable)
		: this(IsStreamable.Default, downloadable) {}

	protected RequestFileResultBase(ICondition<string> streams, Func<RequestFileResultInput, bool> downloadable)
	{
		_streams      = streams;
		_downloadable = downloadable;
	}

	public async ValueTask<IActionResult> Get(Server.Requests.Query<IStorageEntry> parameter)
	{
		var (owner, subject)                           = parameter;
		var (_, _, name, type, _, _, modified, tag, _) = subject.Properties;
		var stop         = owner.HttpContext.RequestAborted;
		var contents     = await subject.Get(stop).Off();
		var entityTag    = new EntityTagHeaderValue(tag.ToString());
		var streamable   = _streams.Get(type);
		var downloadable = _downloadable(new(contents, type, name, modified, entityTag, streamable));
		var address      = await subject.Get(new RelayInput(downloadable ? name : null).Stop(stop)).Off();
		var result       = new RelayRedirectResult(address.ToString());
		return result;
	}
}