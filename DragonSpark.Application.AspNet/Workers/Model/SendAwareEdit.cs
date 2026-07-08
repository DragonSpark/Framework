using DragonSpark.Application.AspNet.Entities.Editing;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;
using DragonSpark.Model.Operations.Stop;
using DragonSpark.Model.Selection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Workers.Model;

public class SendAwareEdit : IEdit
{
	readonly IEdit                             _previous;
	readonly IStopAware<Guid>                  _send;
	readonly IStopAware<ExternalProcess, Guid> _id;

	protected SendAwareEdit(IEdit previous, IStopAware<Guid> send) : this(previous, send, x => x.Id) {}

	protected SendAwareEdit(IEdit previous, IStopAware<Guid> send, ISelect<ExternalProcess, Guid> identity)
		: this(previous, send, identity.Get) {}

	protected SendAwareEdit(IEdit previous, IStopAware<Guid> send, Func<ExternalProcess, Guid> identity)
		: this(previous, send, Start.A.Selection<ExternalProcess>().By.Calling(identity).Operation().Out().AsStop()) {}

	protected SendAwareEdit(IEdit previous, IStopAware<Guid> send, IStopAware<ExternalProcess, Guid> id)
	{
		_previous = previous;
		_send     = send;
		_id       = id;
	}

	public async ValueTask<Edit<ExternalProcess>> Get(Stop<ExternalProcess> parameter)
	{
		var (editor, subject) = await _previous.Off(parameter);
		var id = await _id.Off(new(subject, parameter));
		return new(new Editor(editor, _send.Then().Bind(id.Stop(parameter)).Out()), subject);
	}

	sealed class Editor : Appending, IEditor
	{
		readonly IEditor _previous;

		public Editor(IEditor previous, IOperation send) : base(previous, send) => _previous = previous;

		public void Dispose()
		{
			_previous.Dispose();
		}

		public void Add(object entity)
		{
			_previous.Add(entity);
		}

		public void Attach(object entity)
		{
			_previous.Attach(entity);
		}

		public void Update(object entity)
		{
			_previous.Update(entity);
		}

		public void Remove(object entity)
		{
			_previous.Remove(entity);
		}

		public void Clear()
		{
			_previous.Clear();
		}

		public ValueTask Refresh(object entity) => _previous.Refresh(entity);
	}
}