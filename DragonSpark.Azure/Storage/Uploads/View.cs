using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Server.Requests;
using DragonSpark.Text;

namespace DragonSpark.Azure.Storage.Uploads;

public class View : IView
{
	readonly IEntry            _entry;
	readonly IFormatter<Input> _root;

	protected View(IContainer container, IFormatter<Input> root) : this(container.Entry(), root) {}

	protected View(IEntry entry, IFormatter<Input> root)
	{
		_entry = entry;
		_root  = root;
	}

	public async ValueTask<IStorageEntry?> Get(Stop<Input<ViewInput>> parameter)
	{
		var ((principal, (identifier, name)), stop) = parameter;
		var root   = $"{_root.Get(new(principal, identifier))}/{name}";
		var result = await _entry.Off(new(root, stop));
		return result;
	}
}