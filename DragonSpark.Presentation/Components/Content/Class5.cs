namespace DragonSpark.Presentation.Components.Content
{
	internal class Class5 {}

	/*/*public readonly record struct Templates<T>(RenderFragment<T> ChildContent, RenderFragment NotAssignedTemplate,
	                                           RenderFragment ExceptionTemplate);#1#

	sealed class CurrentFragment : IResult<RenderFragment>
	{
		readonly IResult<RenderFragment?> _success;
		readonly IResult<RenderFragment>  _current;

		public CurrentFragment(IResult<RenderFragment?> success, IResult<RenderFragment> current)
		{
			_success = success;
			_current = current;
		}

		public RenderFragment Get() => _success.Get() ?? _current.Get();
	}

	sealed class Exceptions : ISelecting<Exception, RenderFragment>
	{
		readonly IExceptions    _subject;
		readonly Type           _reportedType;
		readonly RenderFragment _template;

		public Exceptions(IExceptions subject, Type reportedType, RenderFragment template)
		{
			_subject      = subject;
			_reportedType = reportedType;
			_template     = template;
		}

		public async ValueTask<RenderFragment> Get(Exception parameter)
		{
			await _subject.Await(_reportedType, parameter);
			return _template;
		}
	}

	sealed class FragmentMonitor : IResult<RenderFragment>
	{
		readonly IResulting<RenderFragment>      _previous;
		readonly IMutable<Task<RenderFragment>?> _store;
		readonly RenderFragment                  _default;

		public FragmentMonitor(IResulting<RenderFragment> previous, RenderFragment @default)
			: this(previous, new Variable<Task<RenderFragment>?>(), @default) {}

		public FragmentMonitor(IResulting<RenderFragment> previous, IMutable<Task<RenderFragment>?> store,
		                       RenderFragment @default)
		{
			_previous = previous;
			_store    = store;
			_default  = @default;
		}

		public RenderFragment Get()
		{
			var current = _store.Get();
			if (current == null)
			{
				_store.Execute(_previous.Get().AsTask());
				return _default;
			}

			return current.IsCompleted ? current.Result : _default;
		}
	}

	sealed class StoreAwareFragmentLoader : IResulting<RenderFragment>
	{
		readonly IResulting<RenderFragment> _previous;
		readonly IMutable<RenderFragment>   _store;

		public StoreAwareFragmentLoader(IResulting<RenderFragment> previous, IMutable<RenderFragment> store)
		{
			_previous = previous;
			_store    = store;
		}

		public async ValueTask<RenderFragment> Get()
		{
			var result = await _previous.Await();
			_store.Execute(result);
			return result;
		}
	}

	sealed class InstanceFragmentLoader<T> : IResulting<RenderFragment>
	{
		readonly IResulting<T>     _result;
		readonly RenderFragment<T> _instance;
		readonly RenderFragment    _unassigned;

		public InstanceFragmentLoader(IResulting<T> result, RenderFragment<T> instance, RenderFragment unassigned)
		{
			_result     = result;
			_instance   = instance;
			_unassigned = unassigned;
		}

		public async ValueTask<RenderFragment> Get()
		{
			var instance = await _result.Get();
			var result   = instance is not null ? _instance(instance) : _unassigned;
			return result;
		}
	}

	sealed class ExceptionAwareFragmentLoader : IResulting<RenderFragment>
	{
		readonly IResulting<RenderFragment>            _previous;
		readonly ISelecting<Exception, RenderFragment> _report;

		public ExceptionAwareFragmentLoader(IResulting<RenderFragment> previous,
		                                    ISelecting<Exception, RenderFragment> report)
		{
			_previous    = previous;
			_report = report;
		}

		public async ValueTask<RenderFragment> Get()
		{
			try
			{
				return await _previous.Await();
			}
			catch (Exception e)
			{
				return await _report.Await(e);
			}
		}
	}*/
}